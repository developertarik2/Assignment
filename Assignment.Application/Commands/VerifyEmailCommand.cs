using Assignment.Application.Interfaces;
using Assignment.Application.Models.Dtos;
using MediatR;

namespace Assignment.Application.Commands
{
    public class VerifyEmailCommand : IRequest<Result>
    {            
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;

    }

    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
    {
      
      
        private readonly IOtpService _otpService;
    
        public VerifyEmailCommandHandler( 
                                        IOtpService otpService
                                     )
        {
           
           
            _otpService = otpService;
     
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
