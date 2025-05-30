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
    public class CreateUserPinCommand : IRequest<Result>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ICNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public string ConfirmPin { get; set; } = string.Empty;

    }

    public class CreateUserPinCommandHandler : IRequestHandler<CreateUserPinCommand, Result>
    {
        private readonly IFinDbContext _context;
        private readonly IMediator _mediator;
        private readonly IPasswordHash _passwordHash;
        private readonly IValidator<CreateUserPinCommand> _validator;
        private readonly IOtpService _otpService;
        private readonly IBroadcastHandler _broadcastHandler;
        public CreateUserPinCommandHandler(IFinDbContext context, IMediator mediator, IPasswordHash passwordHash,
                                        IPasswordHash passwordNewHash, IOtpService otpService,
                                        IValidator<CreateUserPinCommand> validator, IBroadcastHandler broadcastHandler)
        {
            _context = context;
            _mediator = mediator;
            _passwordHash = passwordHash;
            _validator = validator;
            _otpService = otpService;
            _broadcastHandler = broadcastHandler;
        }
        public async Task<Result> Handle(CreateUserPinCommand request, CancellationToken cancellation)
        {
            var result = new Result();

            try
            {
                if (request.Pin != request.ConfirmPin)
                {
                    result.HasError = true;
                    result.Messages.Add("Unmatched PIN");
                    return result;
                }

                var checkUserExist = await _context.Users
               //.AsNoTracking()
               .FirstOrDefaultAsync(q => q.ICNumber == request.ICNumber, cancellationToken: cancellation);

                //if (checkUserExist)
                //{
                //    result.HasError = false;
                //    result.Messages.Add("Account already existed!");
                //    result.Messages.Add("There is account registrered with this IC Number, Please Login to continue!");

                //    return result;
                //}


                string newPasswordHash = string.Empty;
                string newPasswordSaltHash = string.Empty;



                _passwordHash.CreateHash(request.Pin, ref newPasswordHash,
                  ref newPasswordSaltHash);

                checkUserExist!.PasswordHash=newPasswordHash;
                checkUserExist!.PasswordSalt=newPasswordHash;



                
                    _context.Users.Update(checkUserExist);


                    if (await _context.SaveChangesAsync(cancellation) > 0)
                    {
                        
                       
                        result.HasError = false;
                        result.Messages.Add("Enable the biometric Login");

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
