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
    public class VerifyEmailCommand : IRequest<Result>
    {      
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;

    }

    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
    {
        private readonly IFinDbContext _context;
        private readonly IMediator _mediator;         
        private readonly IOtpService _otpService;
        private readonly IBroadcastHandler _broadcastHandler;
        public VerifyEmailCommandHandler(IFinDbContext context, IMediator mediator, IPasswordHash passwordHash,
                                        IOtpService otpService,
                                       IBroadcastHandler broadcastHandler)
        {
            _context = context;
            _mediator = mediator;
           
            _otpService = otpService;
            _broadcastHandler = broadcastHandler;
        }
        public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellation)
        {
            var result = new Result();

            try
            {
                
                                            
                        
                        var isVerify = await _otpService.VerifyOtpAsync(request.Email,request.Otp, OtpChannel.Email);
                        if(isVerify) 
                        {
                            
                            result.HasError = false;
                            result.Messages.Add("Email verified successfully");

                            return result;
                        }
                       
                        
                   
                        else
                        {
                            result.HasError = true;
                            result.Messages.Add("Incorrect Code, Please enter your code Again");
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
