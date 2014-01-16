﻿using System;
using Moq;
using Neddle.Data;
using Xunit;

namespace Neddle.Tests
{
    public class CourseManagerFixture
    {
        [Fact]
        public void InstantiateWithNullDataProviderThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new CourseManager(null));
        }

        [Fact]
        public void LoadCourseSucceeds()
        {
            Course expected = new Course("Test Course", "TST101", "This is a test course.")
                {
                    Id = Guid.NewGuid()
                };

            Mock<ICourseDataProvider> dataProvider = new Mock<ICourseDataProvider>();
            dataProvider.Setup(o => o.Load(expected.Id)).Returns(expected);

            CourseManager manager = new CourseManager(dataProvider.Object);
            Course actual = manager.LoadCourse(expected.Id);

            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
            dataProvider.Verify(o => o.Load(expected.Id), Times.Once);
        }

        [Fact]
        public void LoadCourseNoMatchReturnsNull()
        {
            Guid courseId = Guid.NewGuid();

            Mock<ICourseDataProvider> dataProvider = new Mock<ICourseDataProvider>();
            dataProvider.Setup(o => o.Load(courseId)).Returns(null as Course);

            CourseManager manager = new CourseManager(dataProvider.Object);
            Course actual = manager.LoadCourse(courseId);

            Assert.Null(actual);
            dataProvider.Verify(o => o.Load(courseId), Times.Once);
        }
    }
}