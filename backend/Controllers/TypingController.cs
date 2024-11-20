//Wusing backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypingController : ControllerBase
    {
        private static readonly Random rand = new Random();

        [HttpGet]
        public ActionResult<String> GetText()
        {
            List<String> texts = new List<String>{};
            texts.Add(new String("test melons please ignore"));
            texts.Add(new String("ignore test melons please"));
            texts.Add(new String("The FitnessGram™ Pacer Test is a multistage aerobic capacity test that progressively gets more difficult as it continues. The 20 meter pacer test will begin in 30 seconds. Line up at the start. The running speed starts slowly, but gets faster each minute after you hear this signal. A single lap should be completed each time you hear this sound. Remember to run in a straight line, and run as long as possible. The second time you fail to complete a lap before the sound, your test is over. The test will begin on the word start. On your mark, get ready, start."));
            String text = texts.ElementAt(rand.Next() % 3);
            return Ok(text);
        }
    }
}
