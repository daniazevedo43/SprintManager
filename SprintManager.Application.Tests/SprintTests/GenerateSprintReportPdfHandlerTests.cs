using Moq;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SprintManager.Application.DTOs;
using SprintManager.Application.Exceptions;
using SprintManager.Application.Handlers.Sprints;
using SprintManager.Application.Interfaces;
using SprintManager.Application.Queries.Sprints;
using SprintManager.Domain.Entities;
using SprintManager.Domain.Enums;
using SprintManager.Exceptions.ExceptionsBase;

namespace SprintManager.Application.Tests.SprintTests
{
    public class GenerateSprintReportPdfHandlerTests
    {
        private readonly Mock<ISprintRepository> _mockSprintRepository;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IWorkItemRepository> _mockWorkItemRepository;
        private readonly Mock<ISprintReportPdf> _mockSprintReportPdf;
        private readonly Mock<IDocument> _mockDocument;
        private readonly GenerateSprintReportPdfHandler _handler;

        public GenerateSprintReportPdfHandlerTests() 
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // Initialize mocks for each test
            _mockSprintRepository = new Mock<ISprintRepository>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockWorkItemRepository = new Mock<IWorkItemRepository>();
            _mockSprintReportPdf = new Mock<ISprintReportPdf>();
            _mockDocument = new Mock<IDocument>();

            // Initialize handler injecting the mocks
            _handler = new GenerateSprintReportPdfHandler(
                _mockSprintRepository.Object,
                _mockProjectRepository.Object,
                _mockWorkItemRepository.Object,
                _mockSprintReportPdf.Object
            );
        }

        [Fact]
        public async Task Handle_GeneratesSprintReportPdfFile_ReturnsPdfFileDTO()
        {
            var command = new GenerateSprintReportPdfCommand
            {
                SprintId = Guid.NewGuid(),
            };

            var projectId = Guid.NewGuid();

            var mockSprint = Mock.Of<Sprint>(s =>
                s.Id == command.SprintId &&
                s.ProjectId == projectId && 
                s.SprintName == "Test sprint" &&
                s.StartDate == new DateTime(2025, 12, 01) &&
                s.EndDate == new DateTime(2025, 12, 29) &&
                s.Description == "Test Description" &&
                s.Status == SprintStatus.Planned
            );

            var mockProject = Mock.Of<Project>(p =>
                p.Id == projectId &&
                p.Name == "Test project" &&
                p.CreationDate == new DateTime(2025, 11, 01) &&
                p.Status == ProjectStatus.Active
            );

            var workItems = new List<WorkItem>()
            {
                new WorkItem(
                    mockProject.Id, 
                    "Test work item", 
                    WorkItemType.Task, 
                    Guid.NewGuid(),
                    mockSprint.Id, 
                    Guid.NewGuid(), 
                    "Test description", 
                    WorkItemPriorityLevel.Critical, 
                    new DateTime(2025, 12, 01), 
                    8
                ),
                new WorkItem(
                    mockProject.Id,
                    "Test work item 2",
                    WorkItemType.Bug,
                    Guid.NewGuid(),
                    mockSprint.Id,
                    Guid.NewGuid(),
                    "Test description 2",
                    WorkItemPriorityLevel.High,
                    new DateTime(2025, 12, 01),
                    8
                )
            };

            var pdfFile = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content().Text("Mock PDF Content");
                });
            });

            var fileName = "test_project_test_sprint_report";

            var pdfFileDTO = new PdfFileDTO
            {
                FileName = fileName,
            };

            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(mockSprint);
            _mockProjectRepository.Setup(r => r.GetByIdAsync(mockSprint.ProjectId)).ReturnsAsync(mockProject);
            _mockWorkItemRepository.Setup(r => r.GetWorkItemsBySprintIdAsync(command.SprintId)).ReturnsAsync(workItems);
            _mockSprintReportPdf.Setup(r => r.Compose(mockSprint, mockProject, workItems)).Returns(pdfFile);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(pdfFileDTO.FileName, result.FileName);
            Assert.NotNull(result.FileBytes);

            // Ensure GetByIdAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);

            // Ensure GetByIdAsync was called exactly once.
            _mockProjectRepository.Verify(r => r.GetByIdAsync(mockSprint.ProjectId), Times.Once);

            // Ensure GetWorkItemsBySprintIdAsync was called exactly once.
            _mockWorkItemRepository.Verify(r => r.GetWorkItemsBySprintIdAsync(command.SprintId), Times.Once);

            // Ensure Compose was called exactly once.
            _mockSprintReportPdf.Verify(r => r.Compose(mockSprint, mockProject, workItems), Times.Once);
        }

        [Fact]
        public async Task VerifySprint_ThrowsException_WhenSprintWasNotFound()
        {
            var command = new GenerateSprintReportPdfCommand
            {
                SprintId = Guid.NewGuid(),
            };

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Sprint with ID {command.SprintId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
        }

        [Fact]
        public async Task VerifyProject_ThrowsException_WhenProjectWasNotFound()
        {
            var command = new GenerateSprintReportPdfCommand
            {
                SprintId = Guid.NewGuid(),
            };

            var projectId = Guid.NewGuid();

            var mockSprint = Mock.Of<Sprint>(s =>
                s.Id == command.SprintId &&
                s.ProjectId == projectId &&
                s.SprintName == "Test sprint" &&
                s.StartDate == new DateTime(2025, 12, 01) &&
                s.EndDate == new DateTime(2025, 12, 29) &&
                s.Description == "Test Description" &&
                s.Status == SprintStatus.Planned
            );

            // Repository's mock configuration
            _mockSprintRepository.Setup(r => r.GetByIdAsync(command.SprintId)).ReturnsAsync(mockSprint);
            _mockProjectRepository.Setup(r => r.GetByIdAsync(mockSprint.ProjectId));

            var exception = await Assert.ThrowsAsync<SprintManagerNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal($"Project with ID {mockSprint.ProjectId} not found.", exception.Message);

            // Ensure GetByIdAsync was called exactly once.
            _mockSprintRepository.Verify(r => r.GetByIdAsync(command.SprintId), Times.Once);
            _mockProjectRepository.Verify(r => r.GetByIdAsync(mockSprint.ProjectId), Times.Once);
        }
    }
}