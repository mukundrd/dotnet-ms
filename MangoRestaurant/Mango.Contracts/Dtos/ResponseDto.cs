﻿namespace Mango.Contracts.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;

        public Object? Result { get; set; }

        public string? DisplayMessage { get; set; }

        public List<string>? ErrorMessages { get; set; }
    }
}
