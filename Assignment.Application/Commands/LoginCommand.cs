using Assignment.Application.Interfaces;
using Assignment.Application.Models.Dtos;
using Assignment.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Commands
{
    public class LoginCommand : IRequest<Result>
    {      
        public string ICNumber { get; set; } = string.Empty;
       

    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
    {
        private readonly IFinDbContext _context;
        private readonly IMediator _mediator;         
        private readonly IOtpService _otpService;
        private readonly IBroadcastHandler _broadcastHandler;
        public LoginCommandHandler(IFinDbContext context, IMediator mediator, IPasswordHash passwordHash,
                                        IOtpService otpService,
                                       IBroadcastHandler broadcastHandler)
        {
            _context = context;
            _mediator = mediator;
           
            _otpService = otpService;
            _broadcastHandler = broadcastHandler;
        }
        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellation)
        {
            var result = new Result();

            try
            {
                var checkUserExist = await _context.Users            
               .FirstOrDefaultAsync(q => q.ICNumber == request.ICNumber, cancellationToken: cancellation);

                if (checkUserExist != null)
                {
                    result.HasError = false;
                    result.Messages.Add("No User found!");
                    result.Messages.Add("Please register first!");

                    return result;
                }

                int otpLength = 4;
                string otp = await _otpService.GenerateOtp(checkUserExist!.Phone!, otpLength, OtpChannel.Sms);
                string message = "The OTP for you to verify " + otp;
                var resultSMS = await _broadcastHandler.SendMessage(checkUserExist.Phone!, message, checkUserExist!.Id);

                result.HasError = false;
                result.Messages.Add("4 digit Code has been sent to " + checkUserExist.Phone);

                return result;







            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Messages.Add("something wrong");
            }

            return result;
        }


    }
}
