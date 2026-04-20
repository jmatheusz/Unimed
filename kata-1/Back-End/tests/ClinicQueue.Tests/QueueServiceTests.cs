using System;
using System.Collections.Generic;
using System.Linq;
using ClinicQueue.Models;
using ClinicQueue.Services;
using Xunit;

namespace ClinicQueue.Tests
{
    public class QueueServiceTests
    {
        private readonly QueueService _service;

        public QueueServiceTests()
        {
            _service = new QueueService();
        }

        [Fact]
        public void Rule4_ElderlyWithMediumUrgency_ShouldBecomeHigh()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient("Young Medium", 30, UrgencyLevel.MEDIA, new TimeSpan(10, 00, 00)),
                new Patient("Elderly Medium", 65, UrgencyLevel.MEDIA, new TimeSpan(10, 05, 00))
            };

            // Act
            var result = _service.OrderQueue(patients);

            // Assert
            Assert.Equal("Elderly Medium", result[0].Name);
            Assert.Equal(UrgencyLevel.ALTA, result[0].UrgencyFinal);
        }

        [Fact]
        public void Rule5_MinorWithAnyUrgency_ShouldGainOneLevel()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient("Adult High", 40, UrgencyLevel.ALTA, new TimeSpan(08, 00, 00)),
                new Patient("Minor High", 15, UrgencyLevel.ALTA, new TimeSpan(08, 10, 00))
            };

            // Act
            var result = _service.OrderQueue(patients);

            // Assert
            Assert.Equal("Minor High", result[0].Name);
            Assert.Equal(UrgencyLevel.CRITICA, result[0].UrgencyFinal);
        }
    }
}
