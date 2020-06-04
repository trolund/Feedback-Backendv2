using System;

public class SERLException : Exception {
    public SERLException () { }

    public SERLException (string message) : base (message) { }

    public SERLException (string message, Exception inner) : base (message, inner) { }
}