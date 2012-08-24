using System;
using System.Collections;
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

        [Fact]
        public void IndexReturnsViewResult()
        {
            var controller = CreateController();
            var result = controller.Index();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void GeneratePasswordReturnsPassword()
        {
            var hashService = Substitute.For<IHashService>();
            var hash = new byte[] {0, 0, 0, 0, 0, 0, 0, 0};
            hashService.Hash(Arg.Any<byte[]>(), Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(ci=>hash);
            var controller = CreateController(hashService: hashService);
            var passwordRequest = new PasswordRequest()
                                      {
                                          HashLength = 64,
                                          Iterations = 1000,
                                          MaximumWordLength = 12,
                                          MinimumWordLength = 6,
                                          PassPhrase = "password",
                                          SaltLength = 16,
                                          SourceType = SourceType.Manual,
                                          WordComplexity = 1000,
                                          WordCount = 0
                                      };
            var password = controller.GeneratePassword(passwordRequest);

            IStructuralEquatable output = password.Model.Hash;
            Assert.True(output.Equals(hash, StructuralComparisons.StructuralEqualityComparer));
        }
    }
}