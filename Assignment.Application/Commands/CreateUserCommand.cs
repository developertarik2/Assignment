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
    public class CreateUserCommand : IRequest<Result>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ICNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IFinDbContext _context;
       
     
        private readonly IValidator<CreateUserCommand> _validator;
        private readonly IOtpService _otpService;
        private readonly IBroadcastHandler _broadcastHandler;
        public CreateUserCommandHandler(IFinDbContext context, 
                                        IPasswordHash passwordNewHash, IOtpService otpService,
                                        IValidator<CreateUserCommand> validator, IBroadcastHandler broadcastHandler)
        {
            _context = context;         
            _validator = validator;
            _otpService = otpService;
            _broadcastHandler = broadcastHandler;
        }
        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellation)
        {
            var result = new Result();

            try
            {
                var validResult = await _validator.ValidateAsync(request, cancellation);
                if (!validResult.IsValid)
                {
                    result.HasError = true;
                    foreach (var error in validResult.Errors)
                    {
                        result.Messages.Add(error.ErrorMessage);
                    }
                    return result;
                }

                var checkUserExist = await _context.Users
               .AsNoTracking()
               .AnyAsync(q => q.ICNumber == request.ICNumber, cancellationToken: cancellation);

                if (checkUserExist)
                {
                    result.HasError = false;
                    result.Messages.Add("Account already existed!");
                    result.Messages.Add("There is account registrered with this IC Number, Please Login to continue!");

                    return result;
                }


              





                var user = new User
                    {
                        IsActive = true,

                    
                        EmailVerified = false,
                        PhoneVerified = false,
                        CustomerName = request.Name,
                        Phone = request.Phone,
                        Email = request.Email,                      
                        ICNumber=request.ICNumber
                    };
                    _context.Users.Add(user);


                    if (await _context.SaveChangesAsync(cancellation) > 0)
                    {
                        int otpLength = 4;
                        string otp = await _otpService.GenerateOtp(request.Phone,otpLength,OtpChannel.Sms);
                        string message = "The OTP for you to verify " + otp;
                        var resultSMS = await _broadcastHandler.SendMessage(request.Phone, message, user!.Id);
                       
                        result.HasError = false;
                        result.Messages.Add("4 digit Code has been sent to "+ request.Phone);

                        return result;
                    }
                    else
                    {
                        result.HasError = true;
                        result.Messages.Add("something wrong");
                    }
                
              
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
