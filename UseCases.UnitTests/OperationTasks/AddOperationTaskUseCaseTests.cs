using Common.ResultOf;
using Domain.Core.Types;
using Domain.Logfiles;
using Domain.OperationTasks;
using Moq;
using UseCases.OperationTasks;

namespace UseCases.UnitTests.OperationTasks;

[TestFixture]
public class AddOperationTaskUseCaseTests
{
    private AddOperationTaskUseCase _sut;
    private Mock<IOperationTasksRepository> _repoTasksMock;
    private Mock<ILogfilesRepository> _repoLogfilesMock;

    [SetUp]
    public void SetUp()
    {
        _repoTasksMock = new Mock<IOperationTasksRepository>();
        _repoLogfilesMock = new Mock<ILogfilesRepository>();
        _sut = new AddOperationTaskUseCase(
            _repoTasksMock.Object,
            _repoLogfilesMock.Object);
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenRepositorySucceeds()
    {
        var command = new AddOperationTaskCommand(
            1,
            0,
            OperationTaskType.Manufacturing,
            OperationTaskResultType.Pass,
            "Error message",
            []);
        var expectedTask = new OperationTask { Type = command.Type, Result = command.Result };
        _repoTasksMock.Setup(r => r.AddAsync(It.IsAny<int>(), It.IsAny<OperationTask>()))
            .ReturnsAsync(ResultOf<OperationTask>.Success(expectedTask));

        var result = await _sut.ExecuteAsync(command);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.SuccessValue.Type, Is.EqualTo(expectedTask.Type));
            Assert.That(result.SuccessValue.Result, Is.EqualTo(expectedTask.Result));
            _repoTasksMock.Verify(r => r.AddAsync(It.IsAny<int>(), It.IsAny<OperationTask>()), Times.Once);
        });
    }

    [Test]
    public async Task ExecuteAsync_ShouldReturnFailure_WhenRepositoryThrows()
    {
        var command = new AddOperationTaskCommand(
            1,
            0,
            OperationTaskType.Manufacturing,
            OperationTaskResultType.Pass,
            "Error message",
            []);
        _repoTasksMock.Setup(r => r.AddAsync(It.IsAny<int>(), It.IsAny<OperationTask>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _sut.ExecuteAsync(command);

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Null);
        });
    }
}