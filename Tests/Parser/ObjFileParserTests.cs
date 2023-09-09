using Contracts.Parser;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Parser;
using System;
using Xunit;

namespace Tests.Parser
{


    public class ObjFileParserTests
    {
        private IObjFileParcer _objFileParser;

        public ObjFileParserTests(IObjFileParcer objFileParser)
        {
            _objFileParser = objFileParser;
        }

        #region ObjFile
        [Fact]
        public void LoadObj_From_File()
        {
            // Arrange
            var objFile = new[]
            {
            "v 0.0 0.0 0.0"
        };

            // Act
            var obj = _objFileParser.ParseObjFile("D:\\Projects\\7thSem\\Graphics\\Renderer\\Tests\\Parser\\TestData\\Mario.obj");

            // Assert
            Assert.NotEmpty(obj.VertexList);
        }
        #endregion

        #region Vertex
        [Fact]
        public void LoadObj_OneVert_OneVertCount()
        {
            // Arrange
            var objFile = new[]
            {
            "v 0.0 0.0 0.0"
        };
            // Act
            var obj = _objFileParser.LoadObj(objFile);

            // Assert
            Assert.Single(obj.VertexList);
        }

        [Fact]
        public void LoadOBj_TwoVerts_TwoVertCount()
        {
            // Arrange
            var objFile = new[]
            {
            "v 0.0 0.0 0.0",
            "v 1.0 1.0 1.0"
        };

            // Act
            var obj = _objFileParser.LoadObj(objFile);

            // Assert
            Assert.Equal(2, obj.VertexList.Count);
        }

        [Fact]
        public void LoadObj_EmptyObj_EmptyObjNoVertsNoFaces()
        {
            // Arrange
            var objFile = new string[] { };

            // Act
            var obj = _objFileParser.LoadObj(objFile);

            // Assert
            Assert.Empty(obj.VertexList);
            Assert.Empty(obj.FaceList);
        }

        [Fact]
        public void LoadObj_NoVertPositions_ThrowsArgumentException()
        {
            // Arrange
            var objFile = new[]
            {
            "v 0.0 0.0 0.0",
            "v"
        };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => _objFileParser.LoadObj(objFile));
        }

        [Fact]
        public void LoadObj_CommaSeperatedVertPositions_ThrowsArgumentException()
        {
            // Arrange
            var objFile = new[]
            {
            // Valid
            "v 0, 0, 0",

            // Invalid
            "v 0.1, 0.1, 0.2",
            "v 0.1, 0.1, 0.3",
            "v 0.1, 0.1, 0.4"
        };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => _objFileParser.LoadObj(objFile));
        }

        [Fact]
        public void LoadObj_LettersInsteadOfPositions_ThrowsArgumentException()
        {
            // Arrange
            var objFile = new[]
            {
            "v a b c"
        };

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => _objFileParser.LoadObj(objFile));
        }
        #endregion
    }
}