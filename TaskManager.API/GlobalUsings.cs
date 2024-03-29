﻿global using CoreApiResponse;
global using Mapster;
global using MapsterMapper;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Serilog;
global using Serilog.Exceptions;
global using Serilog.Sinks.Elasticsearch;
global using Swashbuckle.AspNetCore.Filters;
global using System.ComponentModel.DataAnnotations;
global using System.Net;
global using System.Reflection;
global using System.Security.Claims;
global using System.Text;
global using TaskManager.API.Configurations;
global using TaskManager.API.Extensions;
global using TaskManager.API.Hubs;
global using TaskManager.Application.Attachments.Commands.Create;
global using TaskManager.Application.Attachments.Commands.Delete;
global using TaskManager.Application.Attachments.Queries.GetAttachmentsByIssueId;
global using TaskManager.Application.Comments.Commands.Create;
global using TaskManager.Application.Comments.Commands.Delete;
global using TaskManager.Application.Comments.Commands.Update;
global using TaskManager.Application.Comments.Queries.GetCommentsByIssueId;
global using TaskManager.Application.Dashboards.Queries.GetIssueNumOfAssigneeDashboard;
global using TaskManager.Application.Dashboards.Queries.GetIssuesInProjectDashboard;
global using TaskManager.Application.Dashboards.Queries.GetIssueViewModelDashboard;
global using TaskManager.Application.Epics.Commands.Create;
global using TaskManager.Application.Epics.Commands.Delete;
global using TaskManager.Application.Epics.Commands.Update;
global using TaskManager.Application.Filters.Commands.Create;
global using TaskManager.Application.Filters.Commands.Delete;
global using TaskManager.Application.Filters.Queries.GetFilterViewModelsByUserId;
global using TaskManager.Application.Filters.Queries.GetIssuesByConfiguration;
global using TaskManager.Application.Filters.Queries.GetIssuesByFilterConfiguration;
global using TaskManager.Application.IssueEvents.Queries.GetAll;
global using TaskManager.Application.IssueHistories.Queries.GetByIssueId;
global using TaskManager.Application.Issues.Commands.Create;
global using TaskManager.Application.Issues.Commands.Delete;
global using TaskManager.Application.Issues.Commands.Update;
global using TaskManager.Application.Issues.Queries.GetById;
global using TaskManager.Application.Issues.Queries.GetIssuesBySprintId;
global using TaskManager.Application.Issues.Queries.GetIssuesForProject;
global using TaskManager.Application.IssueTypes.Commands.Create;
global using TaskManager.Application.IssueTypes.Commands.Delete;
global using TaskManager.Application.IssueTypes.Commands.Update;
global using TaskManager.Application.IssueTypes.Queries.GetIssueTypesByProjectId;
global using TaskManager.Application.Labels.Commands.Create;
global using TaskManager.Application.Labels.Commands.Delete;
global using TaskManager.Application.Labels.Commands.Update;
global using TaskManager.Application.Labels.Queries.GetLabelsByProjectId;
global using TaskManager.Application.NotificationIssueEvents.Commands.Create;
global using TaskManager.Application.NotificationIssueEvents.Commands.Delete;
global using TaskManager.Application.NotificationIssueEvents.Commands.Update;
global using TaskManager.Application.PermissionGroups.Commands.Create;
global using TaskManager.Application.PermissionGroups.Commands.Delete;
global using TaskManager.Application.PermissionGroups.Commands.Update;
global using TaskManager.Application.PermissionGroups.Queries.GetPermissionGroupsByProjectId;
global using TaskManager.Application.Priorities.Commands.Create;
global using TaskManager.Application.Priorities.Commands.Delete;
global using TaskManager.Application.Priorities.Commands.Update;
global using TaskManager.Application.Priorities.Queries.GetPrioritiesByProjectId;
global using TaskManager.Application.Projects.Commands.Create;
global using TaskManager.Application.Projects.Commands.Delete;
global using TaskManager.Application.Projects.Commands.Update;
global using TaskManager.Application.Projects.Queries.GetEpicFiltersViewModels;
global using TaskManager.Application.Projects.Queries.GetLabelFiltersViewModels;
global using TaskManager.Application.Projects.Queries.GetProjectByCode;
global using TaskManager.Application.Projects.Queries.GetProjectsByFilter;
global using TaskManager.Application.Projects.Queries.GetSprintFiltersViewModels;
global using TaskManager.Application.Projects.Queries.GetTypeFiltersViewModels;
global using TaskManager.Application.Sprints.Commands.Complete;
global using TaskManager.Application.Sprints.Commands.Create;
global using TaskManager.Application.Sprints.Commands.Delete;
global using TaskManager.Application.Sprints.Commands.Start;
global using TaskManager.Application.Sprints.Commands.Update;
global using TaskManager.Application.Sprints.Queries.GetById;
global using TaskManager.Application.Sprints.Queries.GetSprintsForBoard;
global using TaskManager.Application.StatusCategories.Queries.GetAll;
global using TaskManager.Application.Statuses.Commands.Create;
global using TaskManager.Application.Statuses.Commands.Delete;
global using TaskManager.Application.Statuses.Commands.Update;
global using TaskManager.Application.Statuses.Queries.GetStatusesByProjectId;
global using TaskManager.Application.UserProjects.Commands.Create;
global using TaskManager.Application.UserProjects.Commands.Delete;
global using TaskManager.Application.UserProjects.Commands.Update;
global using TaskManager.Application.UserProjects.Queries.GetMembersOfProject;
global using TaskManager.Application.Users.Commands.ChangePassword;
global using TaskManager.Application.Users.Commands.SignIn;
global using TaskManager.Application.Users.Commands.SignUp;
global using TaskManager.Application.Users.Commands.Update;
global using TaskManager.Application.Users.Commands.UploadPhoto;
global using TaskManager.Application.Versions.Commands.Create;
global using TaskManager.Application.Versions.Commands.Delete;
global using TaskManager.Application.Versions.Commands.Update;
global using TaskManager.Application.Versions.Queries.GetVersionsByProjectId;
global using TaskManager.Core.Core.Errors;
global using TaskManager.Core.Core.Maybes;
global using TaskManager.Core.Core.Results;
global using TaskManager.Core.DTOs;
global using TaskManager.Core.Entities;
global using TaskManager.Core.Extensions;
global using TaskManager.Core.Helper;
global using TaskManager.Core.ViewModel;
global using TaskManager.Infrastructure;
global using TaskManager.Persistence;
global using TaskManager.Persistence.Data;
global using static TaskManager.Core.Core.Maybes.MaybeExtensions;
global using static TaskManager.Core.Extensions.CoreExtensions;
