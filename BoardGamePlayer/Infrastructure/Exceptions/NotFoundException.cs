﻿namespace BoardGamePlayer.Infrastructure.Exceptions;

public class NotFoundException(string message) : Exception(message);
