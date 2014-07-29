using System;
using Xunit;
using Xunit.Extensions;

namespace AHundredDoorsInARow.Tests
{
    public class DoorTests
    {
        [Fact]
        public void Door_Ctor_Sets_The_State_Correctly_According_To_Passed_Parameter()
        {
            Door door = new Door(1, DoorState.Open);
            Assert.Equal(DoorState.Open, door.State);
        }

        [Fact]
        public void Door_Ctor_Sets_The_State_As_Closed_By_Default()
        {
            Door door = new Door(1);
            Assert.Equal(DoorState.Closed, door.State);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Door_Ctor_Should_Throw_ArgumentOutOfRangeException_If_Sequence_Is_Smaller_Than_One(int sequence)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Door(sequence));
        }

        [Fact]
        public void Door_Ctor_Should_Set_Sequence_Correctly()
        {
            int sequence = 25;
            Door door = new Door(sequence);
            Assert.Equal(sequence, door.Sequance);
        }

        [Fact]
        public void Door_Toggle_Should_Toggle_The_Door_State()
        {
            // Arrange
            Door door = new Door(1, DoorState.Open);
            
            // Act
            door.Toggle();

            // Assert
            Assert.Equal(DoorState.Closed, door.State);
        }
    }
}
