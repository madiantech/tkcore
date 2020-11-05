using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys.Json
{
    internal abstract class JsonReader : IDisposable
    {
        protected enum State
        {
            Start,
            Complete,
            Property,
            ObjectStart,
            Object,
            ArrayStart,
            Array,
            Closed,
            PostValue,
            ConstructorStart,
            Constructor,
            Error,
            Finished
        }

        private readonly Stack<JsonTokenType> fStack;

        protected State CurrentState { get; private set; }

        public virtual char QuoteChar { get; protected internal set; }

        public virtual JsonToken TokenType { get; private set; }

        public virtual object Value { get; private set; }

        public virtual Type ValueType { get; private set; }

        public virtual int Depth
        {
            get
            {
                int depth = fStack.Count - 1;
                if (IsStartToken(TokenType))
                    return depth - 1;
                else
                    return depth;
            }
        }

        public JsonReader()
        {
            CurrentState = State.Start;
            fStack = new Stack<JsonTokenType>();
            Push(JsonTokenType.None);
        }

        private void Push(JsonTokenType value)
        {
            fStack.Push(value);
        }

        private JsonTokenType Pop()
        {
            return fStack.Pop();
        }

        private JsonTokenType Peek()
        {
            return fStack.Peek();
        }

        public abstract bool Read();

        protected void SetToken(JsonToken newToken)
        {
            SetToken(newToken, null);
        }

        protected virtual void SetToken(JsonToken newToken, object value)
        {
            TokenType = newToken;

            switch (newToken)
            {
                case JsonToken.StartObject:
                    CurrentState = State.ObjectStart;
                    Push(JsonTokenType.Object);
                    break;
                case JsonToken.StartArray:
                    CurrentState = State.ArrayStart;
                    Push(JsonTokenType.Array);
                    break;
                case JsonToken.StartConstructor:
                    CurrentState = State.ConstructorStart;
                    Push(JsonTokenType.Constructor);
                    break;
                case JsonToken.EndObject:
                    ValidateEnd(JsonToken.EndObject);
                    CurrentState = State.PostValue;
                    break;
                case JsonToken.EndArray:
                    ValidateEnd(JsonToken.EndArray);
                    CurrentState = State.PostValue;
                    break;
                case JsonToken.EndConstructor:
                    ValidateEnd(JsonToken.EndConstructor);
                    CurrentState = State.PostValue;
                    break;
                case JsonToken.PropertyName:
                    CurrentState = State.Property;
                    Push(JsonTokenType.Property);
                    break;
                case JsonToken.Undefined:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.String:
                case JsonToken.Raw:
                    CurrentState = State.PostValue;
                    break;
            }

            JsonTokenType current = Peek();
            if (current == JsonTokenType.Property && CurrentState == State.PostValue)
                Pop();

            if (value != null)
            {
                Value = value;
                ValueType = value.GetType();
            }
            else
            {
                Value = null;
                ValueType = null;
            }
        }

        private void ValidateEnd(JsonToken endToken)
        {
            JsonTokenType currentObject = Pop();

            TkDebug.Assert(currentObject == GetTypeForCloseToken(endToken), string.Format(ObjectUtil.SysCulture,
                "JsonToken {0} 不是 JsonType {1} 的合法闭合标志", endToken, currentObject), this);
        }

        protected void SetStateBasedOnCurrent()
        {
            JsonTokenType currentObject = Peek();

            switch (currentObject)
            {
                case JsonTokenType.Object:
                    CurrentState = State.Object;
                    break;
                case JsonTokenType.Array:
                    CurrentState = State.Array;
                    break;
                case JsonTokenType.Constructor:
                    CurrentState = State.Constructor;
                    break;
                case JsonTokenType.None:
                    CurrentState = State.Finished;
                    break;
                default:
                    TkDebug.ThrowToolkitException("将reader状态设置会原先的对象时，碰到一个并不是期望的JsonType: "
                        + currentObject, this);
                    break;
            }
        }

        internal static bool IsStartToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.StartObject:
                case JsonToken.StartArray:
                case JsonToken.StartConstructor:
                case JsonToken.PropertyName:
                    return true;
                case JsonToken.None:
                case JsonToken.Comment:
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Null:
                case JsonToken.Undefined:
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                case JsonToken.EndConstructor:
                case JsonToken.Date:
                case JsonToken.Raw:
                    return false;
                default:
                    return false;
            }
        }

        private JsonTokenType GetTypeForCloseToken(JsonToken token)
        {
            switch (token)
            {
                case JsonToken.EndObject:
                    return JsonTokenType.Object;
                case JsonToken.EndArray:
                    return JsonTokenType.Array;
                case JsonToken.EndConstructor:
                    return JsonTokenType.Constructor;
                default:
                    TkDebug.ThrowToolkitException("不是一个合法关闭的Json Token: " + token, this);
                    return JsonTokenType.None;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (CurrentState != State.Closed && disposing)
                Close();
        }

        public virtual void Close()
        {
            CurrentState = State.Closed;
            TokenType = JsonToken.None;
            Value = null;
            ValueType = null;
        }
    }
}