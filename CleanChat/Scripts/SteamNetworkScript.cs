using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;
using static System.Net.Mime.MediaTypeNames;

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
            t => t.Type is TokenType.Newline && t.AssociatedData == 1
        ], allowPartialMatch: false);

        foreach (var token in tokens) {
            if (localCheckWaiter.Check(token)) {
                yield return token;

                // var CleanChat := get_node("/root/stedeeCleanChat")
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("CleanChat");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/stedeeCleanChat"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 1);

                // if CleanChat.config.Enabled:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("CleanChat");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("config");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("Enabled");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);

                // if not local:
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.OpNot);
                yield return new IdentifierToken("local");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                // var out = []
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);

                // text = "\n" + text
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("\n"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.Newline, 3);

                // GAMECHAT_COLLECTIONS.append(text)
                yield return new IdentifierToken("GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);

                // if GAMECHAT_COLLECTIONS.size() > max_chat_length:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpGreater);
                yield return new IdentifierToken("max_chat_length");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // GAMECHAT_COLLECTIONS.remove(0)
                yield return new IdentifierToken("GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("remove");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);


                // GAMECHAT = ""
                yield return new IdentifierToken("GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 3);

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
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 4);

                // var edited_msg = ""
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 4);

                // if count > 1:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);

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
                yield return new Token(TokenType.Newline, 4);

                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);

                // edited_msg = msg
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 5);

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
                yield return new Token(TokenType.Newline, 3);

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
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 4);

                // GAMECHAT = GAMECHAT + msg
                yield return new IdentifierToken("GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("GAMECHAT");
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 2);

                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 3);

                // var out = []
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("out");
                yield return new Token(TokenType.OpAssign);
                yield return new Token(TokenType.BracketOpen);
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Newline, 3);

                // text = "\n" + "[color=#a4756a][​local​] [/color]" + text
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant("\n"));
                yield return new Token(TokenType.OpAdd);
                yield return new ConstantToken(new StringVariant("[color=#a4756a][local] [/color]"));
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.Newline, 3);

                // LOCAL_GAMECHAT_COLLECTIONS.append(text)
                yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("append");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("text");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);

                // if LOCAL_GAMECHAT_COLLECTIONS.size() > max_chat_length:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("size");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.OpGreater);
                yield return new IdentifierToken("max_chat_length");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 4);

                // LOCAL_GAMECHAT_COLLECTIONS.remove(0)
                yield return new IdentifierToken("LOCAL_GAMECHAT_COLLECTIONS");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("remove");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new IntVariant(0));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 3);

                // LOCAL_GAMECHAT = ""
                yield return new IdentifierToken("LOCAL_GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 3);

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
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 4);

                // var edited_msg = ""
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new ConstantToken(new StringVariant(""));
                yield return new Token(TokenType.Newline, 4);

                // if count > 1:
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("count");
                yield return new Token(TokenType.OpGreater);
                yield return new ConstantToken(new IntVariant(1));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);

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
                yield return new Token(TokenType.Newline, 4);

                // else:
                yield return new Token(TokenType.CfElse);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 5);

                // edited_msg = msg
                yield return new IdentifierToken("edited_msg");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 5);

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
                yield return new Token(TokenType.Newline, 3);

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
                yield return new Token(TokenType.Newline, 4);

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
                yield return new Token(TokenType.Newline, 4);

                // LOCAL_GAMECHAT = LOCAL_GAMECHAT + msg
                yield return new IdentifierToken("LOCAL_GAMECHAT");
                yield return new Token(TokenType.OpAssign);
                yield return new IdentifierToken("LOCAL_GAMECHAT");
                yield return new Token(TokenType.OpAdd);
                yield return new IdentifierToken("msg");
                yield return new Token(TokenType.Newline, 2);

                // emit_signal("_chat_update")
                yield return new IdentifierToken("emit_signal");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("_chat_update"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);

                // return
                yield return new Token(TokenType.CfReturn);

                yield return token;
            }
            else {
                yield return token;
            }
        }
    }
}
