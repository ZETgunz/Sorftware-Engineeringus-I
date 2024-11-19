using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Services
{
    public class GameValidator : Validator<Game>
    {
        public GameValidator()
        {
            AddRule(g => g.Name.Length <= 50, "Game name must not exceed 50 characters");
            AddRule(g => g.Description.Length <= 200, "Game description must not exceed 200 characters");
            AddRule(g => g.Route.StartsWith("/"), "Route must start with /");
        }
    }
}