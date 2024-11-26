using System;
using backend.Models;
using backend.Services;
using backend.Interfaces;
using Xunit;

public class SessionManagerTest
{
    private readonly SessionManager _sessionManager;

    public SessionManagerTest()
    {
        _sessionManager = new SessionManager();
    }

    [Fact]
    public void CreateSession_ShouldReturnSessionId()
    {
        // Arrange
        var session = new UserSession();

        // Act
        var sessionId = _sessionManager.CreateSession(session);

        // Assert
        Assert.False(string.IsNullOrEmpty(sessionId));
    }

    [Fact]
    public void TryGetSession_ShouldReturnTrue_WhenSessionExists()
    {
        // Arrange
        var session = new UserSession();
        var sessionId = _sessionManager.CreateSession(session);

        // Act
        var result = _sessionManager.TryGetSession(sessionId, out var retrievedSession);

        // Assert
        Assert.True(result);
        Assert.NotNull(retrievedSession);
    }

    [Fact]
    public void TryGetSession_ShouldReturnFalse_WhenSessionDoesNotExist()
    {
        // Act
        var result = _sessionManager.TryGetSession("nonexistent", out var retrievedSession);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedSession);
    }

    [Fact]
    public void RemoveSession_ShouldRemoveSession()
    {
        // Arrange
        var session = new UserSession();
        var sessionId = _sessionManager.CreateSession(session);

        // Act
        _sessionManager.RemoveSession(sessionId);
        var result = _sessionManager.TryGetSession(sessionId, out var retrievedSession);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedSession);
    }

    [Fact]
    public void TryGetSession_ShouldUpdateLastAccessed()
    {
        // Arrange
        var session = new UserSession();
        var sessionId = _sessionManager.CreateSession(session);
        var initialLastAccessed = session.LastAccessed;

        // Act
        _sessionManager.TryGetSession(sessionId, out var retrievedSession);
        var updatedLastAccessed = retrievedSession.LastAccessed;

        // Assert
        Assert.True(updatedLastAccessed > initialLastAccessed);
    }
}