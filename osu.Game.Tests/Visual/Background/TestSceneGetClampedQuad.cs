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
        public void Test_getClampedQuad_ClampAxesX()
        {
            // Arrange
            Axes clampAxes = Axes.X;
            Vector2 topLeft = new Vector2(-0.5f, 0.2f);
            Vector2 size = new Vector2(1.5f, 0.5f);


            // Act
            Quad result = TrianglesV2.TrianglesDrawNode.getClampedQuad(clampAxes, topLeft, size);

            // Assert
            Assert.AreEqual(0f, result.TopLeft.X, 0.001);
            Assert.AreEqual(1f, result.Size.X, 0.001);
            Assert.AreEqual(topLeft.Y, result.TopLeft.Y, 0.001);
            Assert.AreEqual(size.Y, result.Size.Y, 0.001);
        }

        [Test]
        public void Test_getClampedQuad_ClampAxesY()
        {
            // Arrange
            Axes clampAxes = Axes.Y;
            Vector2 topLeft = new Vector2(0.3f, -0.8f);
            Vector2 size = new Vector2(0.7f, 2.0f);

            // Act
            Quad result = TrianglesV2.TrianglesDrawNode.getClampedQuad(clampAxes, topLeft, size);

            // Assert
            Assert.AreEqual(topLeft.X, result.TopLeft.X, 0.001);
            Assert.AreEqual(0f, result.TopLeft.Y, 0.001);
            Assert.AreEqual(size.X, result.Size.X, 0.001);
            Assert.AreEqual(1f, result.Size.Y, 0.001);
        }

        [Test]
        public void Test_getClampedQuad_ClampAxesBoth()
        {
            // Arrange
            Axes clampAxes = Axes.Both;
            Vector2 topLeft = new Vector2(-0.5f, -0.8f);
            Vector2 size = new Vector2(1.5f, 2.0f);

            // Act
            Quad result = TrianglesV2.TrianglesDrawNode.getClampedQuad(clampAxes, topLeft, size);

            // Assert
            Assert.AreEqual(0f, result.TopLeft.X, 0.001);
            Assert.AreEqual(0f, result.TopLeft.Y, 0.001);
            Assert.AreEqual(1f, result.Size.X, 0.001);
            Assert.AreEqual(1f, result.Size.Y, 0.001);
        }

        [Test]
        public void Test_getClampedQuad_ClampAxesNone()
        {
            // Arrange
            Axes clampAxes = Axes.None;
            Vector2 topLeft = new Vector2(0.3f, 0.2f);
            Vector2 size = new Vector2(0.4f, 0.3f);

            // Act
            Quad result = TrianglesV2.TrianglesDrawNode.getClampedQuad(clampAxes, topLeft, size);

            // Assert
            Assert.AreEqual(topLeft.X, result.TopLeft.X, 0.001);
            Assert.AreEqual(topLeft.Y, result.TopLeft.Y, 0.001);
            Assert.AreEqual(size.X, result.Size.X, 0.001);
            Assert.AreEqual(size.Y, result.Size.Y, 0.001);
        }
    }

}
