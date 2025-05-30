using Assignment.Application.Interfaces;
using Assignment.Application.Models.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Commands
{
    public class CreateUserPinCommand : IRequest<Result>
    {
  
        public string ICNumber { get; set; } = string.Empty;   
        public string Pin { get; set; } = string.Empty;
        public string ConfirmPin { get; set; } = string.Empty;

    }

    public class CreateUserPinCommandHandler : IRequestHandler<CreateUserPinCommand, Result>
    {
        private readonly IFinDbContext _context;  
        private readonly IPasswordHash _passwordHash;
    
     
        public CreateUserPinCommandHandler(IFinDbContext context,  IPasswordHash passwordHash
                                     
                                     )
        {
            _context = context;
      
            _passwordHash = passwordHash;
        
  
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
                 .FirstOrDefaultAsync(q => q.ICNumber == request.ICNumber, cancellationToken: cancellation);



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
