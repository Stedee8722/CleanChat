using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace CleanChat;
internal class SteamNetworkScript : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // var final_text = ""
        MultiTokenWaiter localCheckWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrVar,
            t => t is IdentifierToken {Name:"final_text"},
            t => t.Type is TokenType.OpAssign,
            t => t is ConstantToken {Value: StringVariant {Value: ""}},
            t => t.Type is TokenType.Newline
        ], allowPartialMatch: false);

        // GAMECHAT = GAMECHAT + msg
        MultiTokenWaiter globalGameChatWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken {Name:"GAMECHAT"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken {Name:"GAMECHAT"},
            t => t.Type is TokenType.OpAdd,
            t => t is IdentifierToken {Name:"msg"},
            t => t.Type is TokenType.Newline && t.AssociatedData == 2
        ], allowPartialMatch: false);

        // LOCAL_GAMECHAT = LOCAL_GAMECHAT + msg
        MultiTokenWaiter localGameChatWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken {Name:"LOCAL_GAMECHAT"},
            t => t.Type is TokenType.OpAssign,
            t => t is IdentifierToken {Name:"LOCAL_GAMECHAT"},
            t => t.Type is TokenType.OpAdd,
            t => t is IdentifierToken {Name:"msg"},
            t => t.Type is TokenType.Newline && t.AssociatedData == 1,
        ], allowPartialMatch: false);

        foreach (var token in tokens) {
            if (localCheckWaiter.Check(token)) {
                yield return token;

                // var out = []
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 2);

                yield return token;
            }
            else if (globalGameChatWaiter.Check(token)) {
                yield return token;

                // GAMECHAT = ""
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 2);

                // for i in range(GAMECHAT_COLLECTIONS.size(), 0, -1):
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpIn);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.GenRange);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                // var msg = GAMECHAT_COLLECTIONS[i - 1]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);

                // var count = GAMECHAT_COLLECTIONS.count(msg)
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);

                // var edited_msg = ""
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 3);

                // if count > 1:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // edited_msg = msg + " [x" + str(count) + "]"
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" [x"));
                yield return new Token(TokenType.OpAdd);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("]"));
                yield return new Token(TokenType.Newline, 3);

                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // edited_msg = msg
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 3);

                // if not out.has(edited_msg):
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("has");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // out.append(str(edited_msg))
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);

                // for i in range(out.size(), 0, -1):
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpIn);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.GenRange);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                // var msg = out[i - 1]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);

                // GAMECHAT = GAMECHAT + msg
                yield return new IdentifierToken("GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("GAMECHAT");
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 1);

                yield return token;
            }
            else if (localGameChatWaiter.Check(token)) {
                yield return token;

                // LOCAL_GAMECHAT = ""
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("LOCAL_GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 2);

                //for i in range(LOCAL_GAMECHAT_COLLECTIONS.size(), 0, -1):
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpIn);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.GenRange);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                // var msg = LOCAL_GAMECHAT_COLLECTIONS[i - 1]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);

                // var count = LOCAL_GAMECHAT_COLLECTIONS.count(msg)
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);

                // var edited_msg = ""
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 3);

                // if count > 1:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // edited_msg = msg + " [x" + str(count) + "]"
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant(" [x"));
                yield return new Token(TokenType.OpAdd);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("]"));
                yield return new Token(TokenType.Newline, 3);

                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // edited_msg = msg
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 3);

                // if not out.has(edited_msg):
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("has");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // out.append(str(edited_msg))
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.TextStr);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);

                // for i in range(out.size(), 0, -1):
                yield return new Token(TokenType.CfFor);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpIn);
                yield return new Token(TokenType.BuiltInFunc, (uint?)BuiltinFunction.GenRange);
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Comma);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.Comma);
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                // var msg = out[i - 1]
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.BracketOpen);
                yield return new IdentifierToken("i");
                yield return new Token(TokenType.OpSub);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);

                // LOCAL_GAMECHAT = LOCAL_GAMECHAT + msg
                yield return new IdentifierToken("LOCAL_GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("LOCAL_GAMECHAT");
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 1);

                yield return token;
            }
            else {
                yield return token;
            }
        }
    }
}
