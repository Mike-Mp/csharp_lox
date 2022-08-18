
public class Token {
   public TokenType type; 
   public String lexeme;
   public Object literal;
   public int line;

   public Token(TokenType type, String lexeme, Object literal, int line) {
    this.type = type;
    this.literal = literal;
    this.lexeme = lexeme;
    this.line = line;
   }

   public String toString() {
    return type + " " + lexeme + " " + literal;
   }
}