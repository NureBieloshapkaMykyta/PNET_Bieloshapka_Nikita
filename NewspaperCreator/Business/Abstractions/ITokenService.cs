using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Business.Abstractions;

public interface ITokenService
{
    string GenerateToken(User user);
}
