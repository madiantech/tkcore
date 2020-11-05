using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using YJC.Toolkit.Sys.Evaluator;

namespace YJC.Toolkit.Sys
{
    internal class Parser
    {
        private string fPstr;
        private int fPtr;

        private readonly Queue<Token> fTokenQueue = new Queue<Token>();
        private readonly Stack<OpToken> fOpStack = new Stack<OpToken>();
        private OperatorCollection fOperators;

        public Parser()
        {
            Initialize();
            TypeRegistry = new TypeRegistry();
        }

        public Parser(string str)
        {
            Initialize();
            fPstr = str;
        }

        public TypeRegistry TypeRegistry { get; set; }

        public object Global { get; set; }

        public string StringToParse
        {
            get
            {
                return fPstr;
            }
            set
            {
                fPstr = value;
                fTokenQueue.Clear();
                fPtr = 0;
            }
        }

        void Initialize()
        {
            fOperators = new OperatorCollection
            {
                {".", new MethodOperator(".", 12, true, OperatorCustomExpressions.MemberAccess)},
                {"!", new UnaryOperator("!", 11, false, Expression.Not, ExpressionType.Not)},
                {"*", new BinaryOperator("*", 10, true, Expression.Multiply, ExpressionType.Multiply)},
                {"/", new BinaryOperator("/", 10, true, Expression.Divide, ExpressionType.Divide)},
                {"%", new BinaryOperator("%", 10, true, Expression.Modulo, ExpressionType.Modulo)},
                {"+", new BinaryOperator("+", 9, true, OperatorCustomExpressions.Add, ExpressionType.Add)},
                {"-", new BinaryOperator("-", 9, true, Expression.Subtract, ExpressionType.Subtract)},
                {"<<", new BinaryOperator("<<", 8, true, Expression.LeftShift, ExpressionType.LeftShift)},
                {">>", new BinaryOperator(">>", 8, true, Expression.RightShift, ExpressionType.RightShift)},
                {"<", new BinaryOperator("<", 7, true, Expression.LessThan, ExpressionType.LessThan)},
                {">", new BinaryOperator(">", 7, true, Expression.GreaterThan, ExpressionType.GreaterThan)},
                {"<=", new BinaryOperator("<=", 7, true, Expression.LessThanOrEqual, ExpressionType.LessThanOrEqual)},
                {">=", new BinaryOperator(">=", 7, true, Expression.GreaterThanOrEqual, ExpressionType.GreaterThanOrEqual)},
                {"==", new BinaryOperator("==", 6, true, Expression.Equal, ExpressionType.Equal)},
                {"!=", new BinaryOperator("!=", 6, true, Expression.NotEqual, ExpressionType.NotEqual)},
                {"&", new BinaryOperator("&", 5, true, Expression.And, ExpressionType.And)},
                {"^", new BinaryOperator("^", 4, true, Expression.ExclusiveOr, ExpressionType.ExclusiveOr)},
                {"|", new BinaryOperator("|", 3, true, Expression.Or, ExpressionType.Or)},
                {"&&", new BinaryOperator("&&", 2, true, Expression.AndAlso, ExpressionType.AndAlso)},
                {"||", new BinaryOperator("||", 1, true, Expression.OrElse, ExpressionType.OrElse)},
                {":", new TernarySeparatorOperator(":", 2, false, OperatorCustomExpressions.TernarySeparator)},
                {"=", new BinaryOperator("=", 1, false, Expression.Assign, ExpressionType.Assign)},
                {"?", new TernaryOperator("?", 1, false, Expression.Condition)}
            };

        }

        /// <summary>
        /// Returns a boolean specifying if the current string pointer is within the bounds of the expression string
        /// </summary>
        /// <returns></returns>
        private bool IsInBounds()
        {
            return fPtr < fPstr.Length;
        }

        public Expression Parse(string expression)
        {
            StringToParse = expression;
            Parse();
            return BuildTree(null, false);
        }

        /// <summary>
        /// Parses the expression and builds the token queue for compiling
        /// </summary>
        public void Parse(bool isScope = false)
        {
            try
            {
                fTokenQueue.Clear();
                fPtr = 0;

                while (IsInBounds())
                {
                    string op = "";

                    int lastptr = fPtr;

                    if (fPstr[fPtr] != ' ')
                    {
                        // Parse enclosed strings
                        if (fPstr[fPtr] == '\'')
                        {
                            bool isStringClosed = false;
                            fPtr++;
                            lastptr = fPtr;
                            var tokenbuilder = new StringBuilder();

                            // check for escaped single-quote and backslash
                            while (IsInBounds())
                            {
                                if (fPstr[fPtr] == '\\')
                                {
                                    tokenbuilder.Append(fPstr.Substring(lastptr, fPtr - lastptr));
                                    char nextchar = fPstr[fPtr + 1];
                                    switch (nextchar)
                                    {
                                        case '\'':
                                        case '\\':
                                            tokenbuilder.Append(nextchar);
                                            break;
                                        default:
                                            TkDebug.ThrowToolkitException("无法识别的转义字符", this);
                                            break;
                                    }
                                    fPtr++;
                                    fPtr++;
                                    lastptr = fPtr;
                                }
                                else if ((fPstr[fPtr] == '\''))
                                {
                                    isStringClosed = true;
                                    break;
                                }
                                else
                                {
                                    fPtr++;
                                }
                            }

                            if (!isStringClosed)
                            {
                                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                    "没有关闭的字符串，位置在{0}", lastptr), this);
                            }

                            tokenbuilder.Append(fPstr.Substring(lastptr, fPtr - lastptr));
                            string token = tokenbuilder.ToString();
                            fTokenQueue.Enqueue(new Token()
                            {
                                Value = token,
                                IsIdent = true,
                                Type = typeof(string)
                            });
                            fPtr++;
                        }
                        // Parse enclosed dates
                        else if (fPstr[fPtr] == '#')
                        {
                            bool isDateClosed = false;

                            fPtr++;
                            lastptr = fPtr;

                            while (IsInBounds())
                            {
                                fPtr++;
                                if (fPstr[fPtr] == '#')
                                {
                                    isDateClosed = true;
                                    break;
                                }
                            }

                            if (!isDateClosed)
                                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                    "未关闭的日期，位置在{0}", lastptr), this);

                            string token = fPstr.Substring(lastptr, fPtr - lastptr);

                            DateTime dt;
                            if (token == "Now")
                            {
                                dt = DateTime.Now;
                            }
                            else
                            {
                                dt = DateTime.Parse(token, ObjectUtil.SysCulture);
                            }

                            fTokenQueue.Enqueue(new Token()
                            {
                                Value = dt,
                                IsIdent = true,
                                Type = typeof(DateTime)
                            });
                            fPtr++;

                        }
                        // ArgSeparator
                        else if (fPstr[fPtr] == ',')
                        {
                            bool pe = false;

                            while (fOpStack.Count > 0)
                            {
                                if ((string)fOpStack.Peek().Value == "(")
                                {
                                    OpToken temp = fOpStack.Pop();
                                    Token lastToken = fOpStack.Peek();
                                    if (lastToken.GetType() == typeof(MemberToken))
                                    {
                                        MemberToken lastmember = (MemberToken)lastToken;
                                        if (lastmember != null) lastmember.ArgCount++;
                                    }
                                    fOpStack.Push(temp);
                                    pe = true;
                                    break;
                                }
                                else
                                {
                                    OpToken popToken = fOpStack.Pop();
                                    fTokenQueue.Enqueue(popToken);
                                }

                            }

                            if (!pe)
                            {
                                TkDebug.ThrowToolkitException("括号不匹配", this);
                            }

                            fPtr++;
                        }
                        // Member accessor
                        else if (fPstr[fPtr] == '.')
                        {
                            if (fOpStack.Count > 0)
                            {
                                OpToken sc = fOpStack.Peek();
                                // if the last operator was also a Member accessor pop it on the tokenQueue
                                if ((string)sc.Value == ".")
                                {
                                    OpToken popToken = fOpStack.Pop();
                                    fTokenQueue.Enqueue(popToken);
                                }
                            }

                            fOpStack.Push(new MemberToken());
                            fPtr++;
                        }
                        // Parse hexadecimal literals
                        else if (HelperMethods.IsHexStart(fPstr, fPtr))
                        {
                            bool isNeg = false;
                            if (fPstr[fPtr] == '-')
                            {
                                isNeg = true;
                                fPtr++;
                                lastptr = fPtr;
                            }
                            //skip 0x
                            fPtr += 2;
                            // Number identifiers start with a number and may contain numbers and decimals
                            while (IsInBounds() && (HelperMethods.IsHex(fPstr, fPtr) || fPstr[fPtr] == 'L'))
                            {
                                fPtr++;
                            }

                            string token = fPstr.Substring(lastptr, fPtr - lastptr);

                            Type ntype = typeof(System.Int32);
                            object val = null;

                            if (token.EndsWith("L", StringComparison.CurrentCulture))
                            {
                                ntype = typeof(System.Int64);
                                token = token.Remove(token.Length - 1, 1);
                            }

                            switch (ntype.Name)
                            {
                                case "Int32":
                                    val = isNeg ? -Convert.ToInt32(token, 16) : Convert.ToInt32(token, 16);
                                    break;
                                case "Int64":
                                    val = isNeg ? -Convert.ToInt64(token, 16) : Convert.ToInt64(token, 16);
                                    break;
                            }

                            fTokenQueue.Enqueue(new Token()
                            {
                                Value = val,
                                IsIdent = true,
                                Type = ntype
                            });
                        }
                        // Parse numbers
                        else if (HelperMethods.IsANumber(fPstr, fPtr))
                        {
                            bool isDecimal = false;
                            int suffixLength = 0;

                            // Number identifiers start with a number and may contain numbers and decimals
                            while (IsInBounds())
                            {
                                if (fPstr[fPtr] == 'l' || fPstr[fPtr] == 'L'
                                    || fPstr[fPtr] == 'u' || fPstr[fPtr] == 'U')
                                {
                                    if (isDecimal)
                                        TkDebug.ThrowToolkitException("不是期望的decimal结束符", this);

                                    //if (suffixLength == 0) suffixStart = _ptr;
                                    if (suffixLength == 1)
                                    {
                                        fPtr++;
                                        break;
                                    }
                                    suffixLength++;
                                }
                                else if (fPstr[fPtr] == '.')
                                {
                                    if (isDecimal) break;
                                    isDecimal = true;
                                }
                                else if (fPstr[fPtr] == 'd' || fPstr[fPtr] == 'D'
                                    || fPstr[fPtr] == 'f' || fPstr[fPtr] == 'F'
                                    || fPstr[fPtr] == 'm' || fPstr[fPtr] == 'M')
                                {
                                    suffixLength++;
                                    fPtr++;
                                    break;
                                }
                                else if (!HelperMethods.IsANumber(fPstr, fPtr))
                                {
                                    break;
                                }
                                fPtr++;
                            }

                            string token = fPstr.Substring(lastptr, fPtr - lastptr);
                            string suffix = "";

                            Type ntype = null;
                            object val = null;

                            if (suffixLength > 0)
                            {
                                suffix = token.Substring(token.Length - suffixLength);
                                token = token.Substring(0, token.Length - suffixLength);

                                switch (suffix.ToLower(ObjectUtil.SysCulture))
                                {
                                    case "d":
                                        ntype = typeof(Double);
                                        val = double.Parse(token, CultureInfo.InvariantCulture);
                                        break;
                                    case "f":
                                        ntype = typeof(Single);
                                        val = float.Parse(token, CultureInfo.InvariantCulture);
                                        break;
                                    case "m":
                                        ntype = typeof(Decimal);
                                        val = decimal.Parse(token, CultureInfo.InvariantCulture);
                                        break;
                                    case "l":
                                        ntype = typeof(Int64);
                                        val = long.Parse(token, CultureInfo.InvariantCulture);
                                        break;
                                    case "u":
                                        ntype = typeof(UInt32);
                                        val = uint.Parse(token, CultureInfo.InvariantCulture);
                                        break;
                                    case "ul":
                                    case "lu":
                                        ntype = typeof(UInt64);
                                        val = ulong.Parse(token, CultureInfo.InvariantCulture);
                                        break;
                                }

                            }
                            else
                            {
                                if (isDecimal)
                                {
                                    ntype = typeof(Double);
                                    val = double.Parse(token, CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    ntype = typeof(Int32);
                                    val = int.Parse(token, CultureInfo.InvariantCulture);
                                }
                            }


                            fTokenQueue.Enqueue(new Token()
                            {
                                Value = val,
                                IsIdent = true,
                                Type = ntype
                            });
                        }
                        // Test for identifier
                        else if (HelperMethods.IsAlpha(fPstr[fPtr]) || (fPstr[fPtr] == '_'))
                        {
                            fPtr++;

                            while (IsInBounds() && (HelperMethods.IsAlpha(fPstr[fPtr])
                                || (fPstr[fPtr] == '_') || HelperMethods.IsNumeric(fPstr, fPtr)))
                            {
                                fPtr++;
                            }

                            string token = fPstr.Substring(lastptr, fPtr - lastptr);


                            MemberToken mToken = null;

                            if (fOpStack.Count > 0)
                            {
                                OpToken opToken = fOpStack.Peek();
                                if (opToken.GetType() == typeof(MemberToken))
                                    mToken = (MemberToken)opToken;
                            }

                            if ((mToken != null) && (mToken.Name == null))
                            {
                                mToken.Name = token;
                            }
                            else if (TypeRegistry.ContainsKey(token))
                            {
                                if (TypeRegistry[token].GetType().Name == "RuntimeType")
                                {
                                    fTokenQueue.Enqueue(new Token()
                                    {
                                        Value = ((Type)TypeRegistry[token]).UnderlyingSystemType,
                                        IsType = true
                                    });
                                }
                                else
                                {
                                    fTokenQueue.Enqueue(new Token()
                                    {
                                        Value = TypeRegistry[token],
                                        IsType = true
                                    });
                                }
                            }
                            else
                            {
                                if ((token == "null"))
                                {
                                    fTokenQueue.Enqueue(new Token()
                                    {
                                        Value = null,
                                        IsIdent = true,
                                        Type = typeof(object)
                                    });
                                }
                                else if ((token == "true") || (token == "false"))
                                {
                                    fTokenQueue.Enqueue(new Token()
                                    {
                                        Value = Boolean.Parse(token),
                                        IsIdent = true,
                                        Type = typeof(Boolean)
                                    });
                                }
                                else
                                {
                                    if (Global != null)
                                    {
                                        fTokenQueue.Enqueue(new Token() { Value = Global, IsType = true });
                                    }
                                    else
                                    {
                                        if (isScope)
                                        {
                                            fTokenQueue.Enqueue(new Token()
                                            {
                                                IsScope = true
                                            });
                                        }
                                        else
                                        {
                                            //_tokenQueue.Enqueue(new Token() { IsIdent = true, Value = token });
                                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                                "未知的类型或标识符'{0}'", token), this);
                                        }
                                    }

                                    if (fOpStack.Count > 0)
                                    {
                                        OpToken sc = fOpStack.Peek();
                                        // if the last operator was also a Member accessor pop it on the tokenQueue
                                        if ((string)sc.Value == ".")
                                        {
                                            OpToken popToken = fOpStack.Pop();
                                            fTokenQueue.Enqueue(popToken);
                                        }
                                    }

                                    fOpStack.Push(new MemberToken());
                                    fPtr -= token.Length;
                                }
                            }
                        }
                        else if (fPstr[fPtr] == '[')
                        {
                            fOpStack.Push(new OpToken()
                            {
                                Value = "[",
                                Ptr = fPtr + 1
                            });
                            fPtr++;
                        }
                        else if (fPstr[fPtr] == ']')
                        {
                            bool pe = false;
                            // Until the token at the top of the stack is a left bracket,
                            // pop operators off the stack onto the output queue
                            while (fOpStack.Count > 0)
                            {
                                OpToken sc = fOpStack.Peek();
                                if ((string)sc.Value == "[")
                                {
                                    OpToken temp = fOpStack.Pop();
                                    if (fOpStack.Count > 0)
                                    {
                                        Token lastToken = fOpStack.Peek();
                                        if (lastToken.GetType() == typeof(MemberToken))
                                        {
                                            MemberToken lastmember = (MemberToken)lastToken;
                                            // check if there was anything significant between the opening paren and the closing paren
                                            // If so, then we have an argument... This isn't the best approach perhaps, but it works...
                                            if (fPstr.Substring(sc.Ptr, fPtr - sc.Ptr).Trim().Length > 0)
                                                lastmember.ArgCount++;
                                        }
                                    }
                                    fOpStack.Push(temp);
                                    pe = true;
                                    break;
                                }
                                else
                                {
                                    OpToken popToken = fOpStack.Pop();
                                    fTokenQueue.Enqueue(popToken);
                                }
                            }

                            // If the stack runs out without finding a left parenthesis, then there are mismatched parentheses.
                            if (!pe)
                            {
                                TkDebug.ThrowToolkitException("括号不匹配", this);
                            }

                            // Pop the left parenthesis from the stack, but not onto the output queue.
                            fOpStack.Pop();
                            //tokenQueue.Enqueue(lopToken);


                            fPtr++;
                        }
                        else if (fPstr[fPtr] == '(')
                        {
                            int curptr = fPtr;
                            int depth = 0;
                            var containsComma = false;

                            while (IsInBounds())
                            {
                                if (fPstr[fPtr] == '(')
                                    depth++;
                                if (fPstr[fPtr] == ')')
                                    depth--;
                                if (fPstr[fPtr] == ',')
                                    containsComma = true;
                                fPtr++;
                                if (depth == 0) break;
                            }

                            fPtr--;

                            if (depth != 0)
                                TkDebug.ThrowToolkitException("括号不匹配", this);

                            string token = fPstr.Substring(lastptr + 1, fPtr - lastptr - 1).Trim();

                            Type t;

                            bool isCast = false;

                            if (!containsComma)
                            {
                                if (TypeRegistry.ContainsKey(token))
                                {
                                    fTokenQueue.Enqueue(new Token()
                                    {
                                        Value = "(" + token + ")",
                                        IsCast = true,
                                        Type = (Type)TypeRegistry[token]
                                    });
                                    //_ptr = curptr + 1;
                                    isCast = true;
                                }
                                else if ((t = Type.GetType(token)) != null)
                                {
                                    fTokenQueue.Enqueue(new Token()
                                    {
                                        Value = "(" + t.Name + ")",
                                        IsCast = true,
                                        Type = t
                                    });
                                    // _ptr = curptr + 1;
                                    isCast = true;
                                }
                            }

                            if (!isCast)
                            {
                                fOpStack.Push(new OpToken()
                                {
                                    Value = "(",
                                    Ptr = curptr + 1
                                });

                                fPtr = curptr + 1;
                            }

                        }
                        else if (fPstr[fPtr] == ')')
                        {
                            bool pe = false;
                            //int poppedtokens = 0;
                            // Until the token at the top of the stack is a left parenthesis,
                            // pop operators off the stack onto the output queue
                            while (fOpStack.Count > 0)
                            {
                                OpToken sc = fOpStack.Peek();
                                if ((string)sc.Value == "(")
                                {
                                    OpToken temp = fOpStack.Pop();
                                    if (fOpStack.Count > 0)
                                    {
                                        Token lastToken = fOpStack.Peek();
                                        if (lastToken.GetType() == typeof(MemberToken))
                                        {
                                            MemberToken lastmember = (MemberToken)lastToken;
                                            // check if there was anything significant between the opening paren and the closing paren
                                            // If so, then we have an argument... This isn't the best approach perhaps, but it works...
                                            if (fPstr.Substring(sc.Ptr, fPtr - sc.Ptr).Trim().Length > 0)
                                                lastmember.ArgCount++;
                                        }
                                    }
                                    fOpStack.Push(temp);
                                    pe = true;
                                    break;
                                }
                                else
                                {
                                    OpToken popToken = fOpStack.Pop();
                                    fTokenQueue.Enqueue(popToken);
                                    // poppedtokens++;
                                }
                            }

                            // If the stack runs out without finding a left parenthesis, then there are mismatched parentheses.
                            if (!pe)
                            {
                                TkDebug.ThrowToolkitException("括号不匹配", this);
                            }

                            // Pop the left parenthesis from the stack, but not onto the output queue.
                            fOpStack.Pop();

                            //If the token at the top of the stack is a function token, pop it onto the output queue.
                            if (fOpStack.Count > 0)
                            {
                                OpToken popToken = fOpStack.Peek();
                                if ((string)popToken.Value == ".")
                                {
                                    popToken = fOpStack.Pop();
                                    popToken.IsFunction = true;
                                    fTokenQueue.Enqueue(popToken);
                                }
                            }
                            fPtr++;
                        }
                        //else if (_pstr[_ptr] == '=' && _pstr[_ptr + 1] == '>')
                        //{
                        //    _ptr++;
                        //    _ptr++;
                        //}
                        else if ((op = fOperators.IsOperator(fPstr, ref fPtr)) != null)
                        {
                            while (fOpStack.Count > 0)
                            {
                                OpToken sc = fOpStack.Peek();

                                if (fOperators.IsOperator((string)sc.Value) &&
                                    ((fOperators[op].LeftAssoc &&
                                      (fOperators[op].Precedence <= fOperators[(string)sc.Value].Precedence)) ||
                                     (fOperators[op].Precedence < fOperators[(string)sc.Value].Precedence))
                                    )
                                {
                                    OpToken popToken = fOpStack.Pop();
                                    fTokenQueue.Enqueue(popToken);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            fOpStack.Push(new OpToken() { Value = op });
                            fPtr++;
                        }
                        else
                        {
                            TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                                "不是期望的标识符 '{0}'", fPstr[fPtr]), this);
                        }
                    }
                    else
                    {
                        fPtr++;
                    }
                }

                while (fOpStack.Count > 0)
                {
                    OpToken sc = fOpStack.Peek();
                    if ((string)sc.Value == "(" || (string)sc.Value == ")")
                    {
                        TkDebug.ThrowToolkitException("括号不匹配", this);
                    }

                    sc = fOpStack.Pop();
                    fTokenQueue.Enqueue(sc);
                }

            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(String.Format(ObjectUtil.SysCulture,
                    "分析错误，位置在{0}: {1}", fPtr, ex.Message), ex, this);
            }
        }

        /// <summary>
        /// Builds the expression tree from the token queue
        /// </summary>
        /// <returns></returns>
        public Expression BuildTree(Expression scopeParam, bool isCall)
        {
            if (fTokenQueue.Count == 0)
                Parse(scopeParam != null);

            // make a copy of the queue, so that we don't empty the original queue
            var tempQueue = new Queue<Token>(fTokenQueue);
            var exprStack = new Stack<Expression>();
            var args = new List<Expression>();
            //var literalStack = new Stack<String>();

#if DEBUG
            var q = tempQueue.Select(x => (x.Value ?? "<null>").ToString()
                + (x.GetType() == typeof(MemberToken) ? ":" + ((MemberToken)x).Name : ""));
            System.Diagnostics.Debug.WriteLine(string.Join("][", q.ToArray()));
#endif
            int isCastPending = -1;
            Type typeCast = null;

            while (tempQueue.Count > 0)
            {
                Token t = tempQueue.Dequeue();
                t.IsCall = isCall && tempQueue.Count == 0;
                if (isCastPending > -1) isCastPending--;
                if (isCastPending == 0)
                {
                    exprStack.Push(Expression.Convert(exprStack.Pop(), typeCast));
                    isCastPending = -1;
                }

                if (t.IsIdent)
                {
                    // handle numeric literals
                    exprStack.Push(Expression.Constant(t.Value, t.Type));
                }
                else if (t.IsType)
                {
                    exprStack.Push(Expression.Constant(t.Value));
                }
                else if (t.IsScope)
                {
                    if (scopeParam == null)
                    {
                        TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                            "不是期望的标识符 {0}或者作用域为空", t.Value), this);
                    }
                    exprStack.Push(scopeParam);
                }
                else if (t.IsOperator)
                {
                    // handle operators
                    Expression result = null;
                    var op = fOperators[(string)t.Value];
                    var opfunc = OpFuncServiceLocator.Resolve(op.GetType());
                    for (int i = 0; i < t.ArgCount; i++)
                    {
                        args.Add(exprStack.Pop());
                    }
                    // Arguments are in reverse order
                    args.Reverse();
                    result = opfunc(new OpFuncArgs()
                        {
                            TempQueue = tempQueue,
                            ExprStack = exprStack,
                            T = t,
                            Op = op,
                            Args = args,
                            ScopeParam = scopeParam,
                            Types = new List<string>() { "System.Linq" }
                        });
                    args.Clear();
                    exprStack.Push(result);
                }
                else if (t.IsCast)
                {
                    isCastPending = 2;
                    typeCast = t.Type;
                }
            }

            // we should only have one complete expression on the stack, otherwise, something went wrong
            if (exprStack.Count == 1)
            {
                Expression pop = exprStack.Pop();
#if DEBUG
                System.Diagnostics.Debug.WriteLine(pop.ToString());
#endif
                return pop;
            }
            else
            {
                TkDebug.ThrowToolkitException("非法的表达式", this);
                return null;
            }
        }

    }
}
