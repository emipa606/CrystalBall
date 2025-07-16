using Verse;

namespace Crystalball;

[StaticConstructorOnStartup]
public static class CrystalBallStatic
{
    public static CrystalBallMod currMod = null;

    static CrystalBallStatic() //constructor is called before anything is loaded in
    {
    }
}