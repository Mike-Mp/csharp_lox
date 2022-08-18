
public class Scanner
{
    String source;
    List<Token> tokens = new List<Token>();
    int start = 0;
    int current = 0;
    int line = 1;
    Dictionary<String, TokenType> keywords = new Dictionary<String, TokenType>();

    public Scanner(String source)
    {
        this.source = source;

        keywords["and"] = TokenType.AND;
        keywords["class"] = TokenType.CLASS;
        keywords["else"] = TokenType.ELSE;
        keywords["false"] = TokenType.FALSE;
        keywords["true"] = TokenType.TRUE;
        keywords["for"] = TokenType.FOR;
        keywords["fun"] = TokenType.FUN;
        keywords["if"] = TokenType.IF;
        keywords["nil"] = TokenType.NIL;
        keywords["or"] = TokenType.OR;
        keywords["print"] = TokenType.PRINT;
        keywords["return"] = TokenType.RETURN;
        keywords["super"] = TokenType.SUPER;
        keywords["this"] = TokenType.THIS;
        keywords["var"] = TokenType.VAR;
        keywords["while"] = TokenType.WHILE;
    }

    public List<Token> scanTokens()
    {
        while (!isAtEnd())
        {
            start = current;
            scanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    public void scanToken()
    {
        char c = advance();
        switch (c)
        {
            case '(': addToken(TokenType.LEFT_PAREN); break;
            case ')': addToken(TokenType.RIGHT_PAREN); break;
            case '{': addToken(TokenType.LEFT_BRACE); break;
            case '}': addToken(TokenType.RIGHT_BRACE); break;
            case ',': addToken(TokenType.COMMA); break;
            case '.': addToken(TokenType.DOT); break;
            case '-': addToken(TokenType.MINUS); break;
            case '+': addToken(TokenType.PLUS); break;
            case ';': addToken(TokenType.SEMICOLON); break;
            case '*': addToken(TokenType.STAR); break;
            case '!':
                addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (match('/'))
                {
                    // A comment goes until the end of the line.
                    while (peek() != '\n' && !isAtEnd()) advance();
                }
                else if (match('*'))
                {
                    while ((peek() != '*' && peekNext() != '/') && !isAtEnd())
                    {
                        if (peek() == '\n') line++;
                        // Console.WriteLine("1peek() == {0} 1peekNext() == {1}", peek(), peekNext());
                        advance();
                    }
                    // Console.WriteLine("peek() == {0} peekNext() == {1}", peek(), peekNext());
                    current += 2;
                }
                else
                {
                    addToken(TokenType.SLASH);
                }
                break;

            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;

            case '\n':
                line++;
                break;
            case '"': string_fun(); break;
            default:
                if (isDigit(c))
                {
                    number();
                }
                else if (isAlpha(c))
                {
                    identifier();
                }
                else
                {
                    String characterAndError = String.Format("('{0}') Unexpected character", c);
                    Lox.error(line, characterAndError);
                }
                break;
        }
    }

    private void identifier()
    {
        while (isAlphaNumeric(peek())) advance();

        String text = source.Substring(start, current - start);
        TokenType type;
        if (keywords.ContainsKey(text))
        {
            type = keywords[text];
        }
        else
        {
            type = TokenType.IDENTIFIER;
        }

        addToken(type);
    }

    private Boolean isAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    private Boolean isAlphaNumeric(char c)
    {
        return isAlpha(c) || isDigit(c);
    }

    private Boolean isDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private void number()
    {
        while (isDigit(peek())) advance();

        if (peek() == '.' && isDigit(peekNext()))
        {
            advance();

            while (isDigit(peek())) advance();
        }


        addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
    }

    private char peekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    private void string_fun()
    {
        while (peek() != '"' && !isAtEnd())
        {
            if (peek() == '\n') line++;
            advance();
        }

        if (isAtEnd())
        {
            Lox.error(line, "Unterminated string.");
            return;
        }

        advance();

        String value = source.Substring(start, current - start);
        addToken(TokenType.STRING, value);
    }

    private char peek()
    {
        if (isAtEnd()) return '\0';
        return source[current];
    }

    private Boolean match(char expected)
    {
        if (isAtEnd()) return false;
        if (expected != source[current]) return false;

        current++;
        return true;
    }

    private char advance()
    {
        return source[current++];
    }

    private void addToken(TokenType type)
    {
        addToken(type, null);
    }

    private void addToken(TokenType type, Object literal)
    {
        String text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal, line));
    }

    public Boolean isAtEnd()
    {
        return current >= source.Length;
    }
}