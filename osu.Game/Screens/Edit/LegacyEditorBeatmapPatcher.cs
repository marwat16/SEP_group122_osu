// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using DiffPlex;
using DiffPlex.Model;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Skinning;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;

namespace osu.Game.Screens.Edit
{
    /// <summary>
    /// Patches an <see cref="EditorBeatmap"/> based on the difference between two legacy (.osu) states.
    /// </summary>
    public class LegacyEditorBeatmapPatcher
    {
        private readonly EditorBeatmap editorBeatmap;

        public LegacyEditorBeatmapPatcher(EditorBeatmap editorBeatmap)
        {
            this.editorBeatmap = editorBeatmap;
        }

        public void Patch(byte[] currentState, byte[] newState)
        {
            // Diff the beatmaps
            var result = new Differ().CreateLineDiffs(readString(currentState), readString(newState), true, false);
            IBeatmap newBeatmap = null;

            editorBeatmap.BeginChange();
            processHitObjects(result, () => newBeatmap ??= readBeatmap(newState));
            processTimingPoints(result, () => newBeatmap ??= readBeatmap(newState));
            editorBeatmap.EndChange();
        }

        private void processTimingPoints(DiffResult result, Func<IBeatmap> getNewBeatmap)
        {
            findChangedIndices(result, LegacyDecoder<Beatmap>.Section.TimingPoints, out var removedIndices, out var addedIndices);

            if (removedIndices.Count == 0 && addedIndices.Count == 0)
                return;

            // Due to conversion from legacy to non-legacy control points, it becomes difficult to diff control points correctly.
            // So instead _all_ control points are reloaded if _any_ control point is changed.

            var newControlPoints = EditorBeatmap.ConvertControlPoints(getNewBeatmap().ControlPointInfo);

            editorBeatmap.ControlPointInfo.Clear();
            foreach (var point in newControlPoints.AllControlPoints)
                editorBeatmap.ControlPointInfo.Add(point.Time, point);
        }

        private void processHitObjects(DiffResult result, Func<IBeatmap> getNewBeatmap)
        {
            findChangedIndices(result, LegacyDecoder<Beatmap>.Section.HitObjects, out var removedIndices, out var addedIndices);

            foreach (int removed in removedIndices)
                editorBeatmap.RemoveAt(removed);

            if (addedIndices.Count > 0)
            {
                var newBeatmap = getNewBeatmap();

                foreach (int i in addedIndices)
                    editorBeatmap.Insert(i, newBeatmap.HitObjects[i]);
            }
        }

        private void findChangedIndices(DiffResult result, LegacyDecoder<Beatmap>.Section section, out List<int> removedIndices, out List<int> addedIndices)
        {
            removedIndices = new List<int>();
            addedIndices = new List<int>();

            // Find the index of [HitObject] sections. Lines changed prior to this index are ignored.
            int oldSectionStartIndex = Array.IndexOf(result.PiecesOld, $"[{section}]");
            if (oldSectionStartIndex == -1)
                return;

            int oldSectionEndIndex = Array.FindIndex(result.PiecesOld, oldSectionStartIndex + 1, s => s.StartsWith('['));
            if (oldSectionEndIndex == -1)
                oldSectionEndIndex = result.PiecesOld.Length;

            int newSectionStartIndex = Array.IndexOf(result.PiecesNew, $"[{section}]");
            if (newSectionStartIndex == -1)
                return;

            int newSectionEndIndex = Array.FindIndex(result.PiecesNew, newSectionStartIndex + 1, s => s.StartsWith('['));
            if (newSectionEndIndex == -1)
                newSectionEndIndex = result.PiecesNew.Length;

            Debug.Assert(oldSectionStartIndex >= 0);
            Debug.Assert(newSectionStartIndex >= 0);

            foreach (var block in result.DiffBlocks)
            {
                // Removed indices
                for (int i = 0; i < block.DeleteCountA; i++)
                {
                    int objectIndex = block.DeleteStartA + i;

                    if (objectIndex <= oldSectionStartIndex || objectIndex >= oldSectionEndIndex)
                        continue;

                    removedIndices.Add(objectIndex - oldSectionStartIndex - 1);
                }

                // Added indices
                for (int i = 0; i < block.InsertCountB; i++)
                {
                    int objectIndex = block.InsertStartB + i;

                    if (objectIndex <= newSectionStartIndex || objectIndex >= newSectionEndIndex)
                        continue;

                    addedIndices.Add(objectIndex - newSectionStartIndex - 1);
                }
            }

            // Sort the indices to ensure that removal + insertion indices don't get jumbled up post-removal or post-insertion.
            // This isn't strictly required, but the differ makes no guarantees about order.
            removedIndices.Sort();
            addedIndices.Sort();

            // The expected usage of this returned list is to iterate from the start to the end of the list, such that
            // these indices need to appear in reverse order for the usage to not have to deal with decrementing indices.
            removedIndices.Reverse();
        }

        private string readString(byte[] state) => Encoding.UTF8.GetString(state);

        private IBeatmap readBeatmap(byte[] state)
        {
            using (var stream = new MemoryStream(state))
            using (var reader = new LineBufferedReader(stream, true))
            {
                var decoded = Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
                decoded.BeatmapInfo.Ruleset = editorBeatmap.BeatmapInfo.Ruleset;
                return new PassThroughWorkingBeatmap(decoded).GetPlayableBeatmap(editorBeatmap.BeatmapInfo.Ruleset);
            }
        }

        private class PassThroughWorkingBeatmap : WorkingBeatmap
        {
            private readonly IBeatmap beatmap;

            public PassThroughWorkingBeatmap(IBeatmap beatmap)
                : base(beatmap.BeatmapInfo, null)
            {
                this.beatmap = beatmap;
            }

            protected override IBeatmap GetBeatmap() => beatmap;

            protected override Texture GetBackground() => throw new NotImplementedException();

            protected override Track GetBeatmapTrack() => throw new NotImplementedException();

            protected internal override ISkin GetSkin() => throw new NotImplementedException();

            public override Stream GetStream(string storagePath) => throw new NotImplementedException();
        }
    }
}
