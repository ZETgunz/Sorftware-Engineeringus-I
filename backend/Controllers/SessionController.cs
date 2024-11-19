using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly SessionManager _sessionManager;

        public SessionController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }
        [HttpPost("create")]
        public IActionResult CreateSession([FromBody] UserSession session)
        {
            var sessionId = _sessionManager.CreateSession(session);

            Response.Cookies.Append("SessionId", sessionId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddMinutes(30)
            });

            return Ok(new { SessionId = sessionId });
        }

        [HttpGet("get")]
        public IActionResult GetSession()
        {
            if (Request.Cookies.TryGetValue("SessionId", out var sessionId) &&
                _sessionManager.TryGetSession(sessionId, out var session))
            {
                return Ok(session);
            }

            return Unauthorized("Session not found or expired");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (Request.Cookies.TryGetValue("SessionId", out var sessionId))
            {
                _sessionManager.RemoveSession(sessionId);
                Response.Cookies.Delete("SessionId");
            }

            return Ok("Logged out");
        }
    }
}