using Moq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.tests.Controller
{
    class ToDoAppControllerTests
    {
        public async Task Index_ReturnsAViewResult_WithTodos()
        {
            var mockRepo = new Mock<IToDoRepository>();

        }
    }
}
