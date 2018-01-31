using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Api
{
    [Route("[controller]")]
    public class QuizController : Controller
    {
        private readonly IDocumentStore _eventStore;
        private readonly QuizAppService _quizAppService;

        public QuizController(QuizAppService quizAppService, IDocumentStore eventStore) 
        {
            _eventStore = eventStore;
            _quizAppService = quizAppService;
        }

        [HttpGet]
        public async Task<object> Get() 
        {
            var state = await _quizAppService.GetState();
            if (state == null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        [HttpGet("{quizId}")]
        public async Task<object> Get(Guid quizId) => 
            await _quizAppService.GetState(quizId);

        [HttpPut]
        [Route("{quizId}")]
        public async Task<object> Answer(Guid quizId, [FromBody]QuizAnswersCommand quizAnswersComand) =>
            await _quizAppService.Answer(new QuizAnswersCommand(quizId, quizAnswersComand.Answers));

        [HttpPost]
        public async Task<object> Start([FromBody]QuizModel quizModel) => 
            await _quizAppService.Start(quizModel);

        [HttpDelete]
        [Route("{quizId}")]
        public async Task<object> Close(Guid quizId) =>
            await _quizAppService.Close(quizId);
    }
}
