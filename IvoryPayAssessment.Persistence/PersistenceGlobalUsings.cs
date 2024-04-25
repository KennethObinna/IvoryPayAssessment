global using System;
global using System.Linq;
global using System.Collections.Generic;
global using System.Text;
global using System.Threading.Tasks;
global using Microsoft.Extensions.DependencyInjection;

global using Mapster;
global using IvoryPayAssessment.Application;
global using IvoryPayAssessment.Application.Mappings;
global using IvoryPayAssessment.Application.Interfacses;

global using Microsoft.AspNetCore.Mvc;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using System.Text.Json.Serialization;
global using FluentValidation.AspNetCore;
global using Newtonsoft;
global using Newtonsoft.Json;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Storage;
global using IvoryPayAssessment.Application.Common.Helpers;
global using IvoryPayAssessment.Application.Implementations;
global using Serilog;
global using IvoryPayAssessment.Domain.Entities;
global using IvoryPayAssessment.Persistence.DataContexts;
global using IvoryPayAssessment.Application.Implementations.UserAccounts;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using IvoryPayAssessment.Application.Interfacses.UserAccounts;
global using IvoryPayAssessment.Application.Common.Models;
global using Microsoft.Data.SqlClient;
global using IvoryPayAssessment.Application.Helpers;
 
global using IvoryPayAssessment.Application.Implementations.UserSessions;
global using IvoryPayAssessment.Application.Interfacses.UserSessions;
global using IvoryPayAssessment.Application.Implementations.UserAccount;
 
global using IvoryPayAssessment.Application.Implementations.OTPServices;
global using IvoryPayAssessment.Application.Interfacses.OTPServices; 
global using IvoryPayAssessment.Application.Common.DTOs;
 
global using Microsoft.AspNetCore.Authentication;


