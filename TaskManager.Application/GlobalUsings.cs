﻿global using CloudinaryDotNet.Actions;
global using Mapster;
global using MapsterMapper;
global using MediatR;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using TaskManager.Application.Core.Abstractions;
global using TaskManager.Application.Core.AzureStorageFileShares;
global using TaskManager.Application.Core.CQRS;
global using TaskManager.Application.Core.Emails;
global using TaskManager.Application.Core.JwtTokens;
global using TaskManager.Application.Core.TextToImages;
global using TaskManager.Application.Mapping;
global using TaskManager.Core;
global using TaskManager.Core.Constants;
global using TaskManager.Core.Core;
global using TaskManager.Core.Core.DomainEvents;
global using TaskManager.Core.Core.Maybes;
global using TaskManager.Core.Core.Results;
global using TaskManager.Core.DomainErrors;
global using TaskManager.Core.DTOs;
global using TaskManager.Core.Entities;
global using TaskManager.Core.Events.Attachments;
global using TaskManager.Core.Events.Comments;
global using TaskManager.Core.Events.Epics;
global using TaskManager.Core.Events.Issues;
global using TaskManager.Core.Events.Projects;
global using TaskManager.Core.Events.Statuses;
global using TaskManager.Core.Events.Users;
global using TaskManager.Core.Exceptions;
global using TaskManager.Core.Extensions;
global using TaskManager.Core.Helper;
global using TaskManager.Core.Interfaces.Repositories;
global using TaskManager.Core.ViewModel;
global using Error = TaskManager.Core.Core.Errors.Error;
global using Version = TaskManager.Core.Entities.Version;
