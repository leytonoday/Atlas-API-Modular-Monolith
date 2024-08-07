﻿using Atlas.Shared.Application.Abstractions.Messaging.Command;

namespace Atlas.Users.Application.CQRS.Users.Commands.ResetPassword;

public sealed record ResetPasswordCommand(string UserName, string NewPassword, string Token) : ICommand;