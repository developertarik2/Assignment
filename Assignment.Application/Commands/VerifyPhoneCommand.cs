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
    public class VerifyPhoneCommand : IRequest<Result>
    {      
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;

    }

    public class VerifyPhoneCommandHandler : IRequestHandler<VerifyPhoneCommand, Result>
    {
        private readonly IFinDbContext _context;
        private readonly IMediator _mediator;         
        private readonly IOtpService _otpService;
        private readonly IBroadcastHandler _broadcastHandler;
        public VerifyPhoneCommandHandler(IFinDbContext context, IMediator mediator, IPasswordHash passwordHash,
                                        IOtpService otpService,
                                       IBroadcastHandler broadcastHandler)
        {
            _context = context;
            _mediator = mediator;
           
            _otpService = otpService;
            _broadcastHandler = broadcastHandler;
        }
        public async Task<Result> Handle(VerifyPhoneCommand request, CancellationToken cancellation)
        {
            var result = new Result();

            try
            {
                
                                            
                        
                        var isVerify = await _otpService.VerifyOtpAsync(request.Phone,request.Otp, OtpChannel.Sms);
                        if(isVerify) 
                        {
                            int otpLength = 4;
                            string otp = await _otpService.GenerateOtp(request.Email,otpLength,OtpChannel.Email);
                            string message = "The OTP for you to verify " + otp;

                            string subject = "Email Verification";

                            var resultSMS = await _broadcastHandler.SendEmail(request.Email,subject, message);
                            result.HasError = false;
                            result.Messages.Add("4 digit Code has been sent to " + request.Email);

                            return result;
                        }
                       
                        
                   
                        else
                        {
                            result.HasError = true;
                            result.Messages.Add("Incorrect OTP, Please enter your OTP Again");
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
