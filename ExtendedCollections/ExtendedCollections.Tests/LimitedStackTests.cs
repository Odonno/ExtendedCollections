﻿using System;
using Xunit;

namespace ExtendedCollections.Tests
{
    public class LimitedStackTests
    {
        [Fact]
        public void CannotExceed5Items()
        {
            // Arrange
            int stackLimit = 5;
            var stack = new LimitedStack<int>(stackLimit);

            int pushedEvents = 0;
            int poppedEvents = 0;

            stack.Pushed += (sender, e) =>
            {
                pushedEvents++;
                Assert.True(stack.Count <= 5);
            };
            stack.Popped += (sender, e) =>
            {
                poppedEvents++;
                Assert.True(stack.Count <= 5);
            };

            // Act
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);
            stack.Push(5);
            stack.Push(6);

            // Assert
            Assert.Equal(6, pushedEvents);
            Assert.Equal(1, poppedEvents);

            Assert.Equal(5, stack.Values[0]);
            Assert.Equal(1, stack.Values[4]);
        }

        [Fact]
        public void ThrowExceptionIfStackLimitIs0()
        {
            // Arrange
            int stackLimit = 0;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedStack<int>(stackLimit));
        }

        [Fact]
        public void ThrowExceptionIfStackLimitIsNegative()
        {
            // Arrange
            int stackLimit = -10;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedStack<int>(stackLimit));
        }

        [Fact]
        public void CanPopSuccessfully()
        {
            // Arrange
            int stackLimit = 5;
            var stack = new LimitedStack<int>(stackLimit);

            int pushedEvents = 0;
            int poppedEvents = 0;

            stack.Pushed += (sender, e) =>
            {
                pushedEvents++;
            };
            stack.Popped += (sender, e) =>
            {
                poppedEvents++;
            };

            // Act
            stack.Push(1);
            var (success, result) = stack.TryPop();

            // Assert
            Assert.Equal(1, pushedEvents);
            Assert.Equal(1, poppedEvents);

            Assert.True(success);
            Assert.Equal(1, result);
        }

        [Fact]
        public void CannotPop()
        {
            // Arrange
            int stackLimit = 5;
            var stack = new LimitedStack<int>(stackLimit);

            int pushedEvents = 0;
            int poppedEvents = 0;

            stack.Pushed += (sender, e) =>
            {
                pushedEvents++;
            };
            stack.Popped += (sender, e) =>
            {
                poppedEvents++;
            };

            // Act
            var (success, result) = stack.TryPop();

            // Assert
            Assert.Equal(0, pushedEvents);
            Assert.Equal(0, poppedEvents);

            Assert.False(success);
        }
    }
}