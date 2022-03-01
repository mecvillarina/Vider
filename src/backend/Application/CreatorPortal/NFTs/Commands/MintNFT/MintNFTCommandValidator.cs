using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Commands.MintNFT
{
    public class MintNFTCommandValidator : AbstractValidator<MintNFTCommand>
    {
        public MintNFTCommandValidator()
        {
            RuleFor(v => v.Name)
               .NotNull()
               .NotEmpty()
               .MinimumLength(3).WithMessage("Name must be between 3 and 100 characters long.")
               .MaximumLength(100).WithMessage("Name must be between 3 and 100 characters long.");
        }
    }
}
