# Authorization Policies

This document describes the authorization policies used in the TaskManager application.

## Overview

Authorization is based on the `IPermissionService`, which checks if a user has the required permissions to perform an action. The service is designed to be extensible to support various authorization scenarios.

## Permissions

The following permissions are defined:

- `CanAccessProject`: Allows a user to access a project.
- `CanEditTask`: Allows a user to edit a task.
- `IsProjectOwner`: Checks if a user is the owner of a project.

## Implementation

The `IPermissionService` interface is defined in `TaskManager.Application`. The implementation, `PermissionService`, is in `TaskManager.Infrastructure`.

Currently, the `PermissionService` has a mock implementation that grants all permissions to all users. This will be replaced with a proper implementation in the future.
