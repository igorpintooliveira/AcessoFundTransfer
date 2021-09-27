using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBackend.FundTransfer.Application.Command
{
    public class FundTransferCommandValidator : AbstractValidator<FundTransferCommand>
    {
        public FundTransferCommandValidator()
        {
            RuleFor(p => p.transactionId)
                .NotEmpty().WithMessage("{transactionId} is required.")
                .NotNull();

            RuleFor(p => p.accountOrigin)
               .NotEmpty().WithMessage("{accountOrigin} is required.")
               .NotNull();

            RuleFor(p => p.accountDestination)
                .NotEmpty().WithMessage("{accountDestination} is required.")
                .NotNull();

            RuleFor(p => p.value)
                .NotEmpty().WithMessage("{value} is required.")
                .NotNull()
                .GreaterThan(0).WithMessage("{value} should be greater than zero.");
        }
    }
}
