using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Models.Dtos
{
    public interface IResult
    {
        List<string> Messages { get; }
        bool Succeeded { get; set; }
        bool HasError { get; set; }
    }

    public class Result : IResult
    {
        public List<string> Messages { get; set; } = new List<string>();
        public bool HasError { get; set; }
        public bool Succeeded { get; set; }

        public static IResult Fail()
        {
            return new Result
            {
                Succeeded = false
            };
        }
    }
    }
