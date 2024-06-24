// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

//using System;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Graphics.Backgrounds;
using osuTK;

//using osu.Game.Graphics.Backgrounds.TrianglesV2;
using osu.Framework.Graphics.Primitives;


namespace osu.Game.Tests.Visual.Background
{
    public partial class TestSceneGetClampedQuad : OsuTestScene
    {
        [Test]
        public void TestGetClampedQuad_AxesNone()
        {
            var topLeft = new Vector2(-0.5f, -0.5f);
            var size = new Vector2(2f, 2f);
            var result = TrianglesV2.TrianglesDrawNode.getClampedQuad(Axes.None, topLeft, size);
            Assert.AreEqual(new Quad(topLeft.X, topLeft.Y, size.X, size.Y), result);
        }

        [Test]
        public void TestGetClampedQuad_AxesX()
        {
            var topLeft = new Vector2(-0.5f, 0.5f);
            var size = new Vector2(2f, 0.5f);
            var result = TrianglesV2.TrianglesDrawNode.getClampedQuad(Axes.X, topLeft, size);
            var expectedTopLeft = new Vector2(0f, 0.5f);
            var expectedSize = new Vector2(1f, 0.5f);
            Assert.AreEqual(new Quad(expectedTopLeft.X, expectedTopLeft.Y, expectedSize.X, expectedSize.Y), result);
        }

        [Test]
        public void TestGetClampedQuad_AxesY()
        {
            var topLeft = new Vector2(0.5f, -0.5f);
            var size = new Vector2(0.5f, 2f);
            var result = TrianglesV2.TrianglesDrawNode.getClampedQuad(Axes.Y, topLeft, size);
            var expectedTopLeft = new Vector2(0.5f, 0f);
            var expectedSize = new Vector2(0.5f, 1f);
            Assert.AreEqual(new Quad(expectedTopLeft.X, expectedTopLeft.Y, expectedSize.X, expectedSize.Y), result);
        }

        [Test]
        public void TestGetClampedQuad_AxesBoth()
        {
            var topLeft = new Vector2(-0.5f, -0.5f);
            var size = new Vector2(2f, 2f);
            var result = TrianglesV2.TrianglesDrawNode.getClampedQuad(Axes.Both, topLeft, size);
            var expectedTopLeft = new Vector2(0f, 0f);
            var expectedSize = new Vector2(1f, 1f);
            Assert.AreEqual(new Quad(expectedTopLeft.X, expectedTopLeft.Y, expectedSize.X, expectedSize.Y), result);
        }
    }

}
