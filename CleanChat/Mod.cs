using GDWeave;

namespace CleanChat;

public class Mod : IMod {
    public Config Config;
    private IModInterface modInterface;

    public Mod(IModInterface modInterface) {
        // init
        this.modInterface = modInterface;
        this.Config = modInterface.ReadConfig<Config>();

        // register script
        this.modInterface.RegisterScriptMod(new SteamNetworkScript());
        
        Log("general", "Loaded stedee.CleanChat!");
    }

    public void Log(string name, string data) {
        this.modInterface.Logger.Information($"[CleanChat.{name}] {data}");
    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
