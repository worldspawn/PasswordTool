using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NSubstitute;
using PasswordTool.Services;
using PasswordTool.Services.ServiceModels;
using PasswordTool.Web.Controllers;
using PasswordTool.Web.Models;
using Xunit;

namespace PasswordTool.Web.Tests.Controllers
{
    public class PasswordControllerTests
    {
        private PasswordController CreateController(IWordService wordService = null, IHashService hashService = null)
        {
            return new PasswordController(
                wordService ?? Substitute.For<IWordService>(),
                hashService ?? Substitute.For<IHashService>()
                );
        }

        //[Fact]
        //public void IndexCreatesAndReturnsPasswordDetail()
        //{
        //    var wordService = Substitute.For<IWordService>();
        //    var words = new string[] {"first", "second", "third"}.Select(w => new WordItem {Word = w});
        //    var task = Task.Factory.StartNew(() => words);
        //    wordService.RandomWords(Arg.Any<int?>(), Arg.Any<int?>()).Returns(task);

        //    var controller = CreateController(wordService);
        //    var result = controller.Index();

        //    Assert.NotNull(result);
        //    Assert.IsType<ViewResult>(result);

        //    var viewResult = result as ViewResult;
        //    Assert.NotNull(viewResult.Model);
        //    var model = viewResult.Model as Password;

        //    //Assert.Equal(string.Join(string.Empty, words.Select(w=>w.Word).ToArray()), model.OriginalPassword);
        //}
    }
}