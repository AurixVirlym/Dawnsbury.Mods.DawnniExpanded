using Dawnsbury.Core.CharacterBuilder.FeatsDb.Common;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Spellbook;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Targeting;
using Dawnsbury.Core.Mechanics.Targeting.Targets;
using Dawnsbury.Display.Text;
using Dawnsbury.Modding;
using Dawnsbury.Audio;
using Microsoft.Xna.Framework;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.CharacterBuilder.Spellcasting;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Creatures;
using System.Runtime.Serialization.Formatters;
using Dawnsbury.Core.Mechanics.Core;

using Dawnsbury.Core.Possibilities;


namespace Dawnsbury.Mods.DawnniExpanded;


public class SpellConcordantChoir
{
    public static ModdedIllustration Spellillustration = new ModdedIllustration("DawnniburyExpandedAssets/ConcordantChoir.png");
    public static SpellId Id;
    public static CombatAction MakeConcordantChoirSpell(Creature caster, int spellLevel, bool inCombat)
    {
        {

            CombatAction ConcordantChoirSpell = Spells.CreateModern(Spellillustration,
                "Concordant Choir",
            new[] { Trait.Sonic, Trait.Evocation, Trait.Divine, Trait.Occult, DawnniExpanded.DETrait },
                    "You unleash a dangerous consonance of reverberating sound, focusing on a single target or spreading out to damage many foes.",
                    "The number of actions you spend Casting this Spell determines its targets, range, area, and other parameters."
                    + "\r\n\r\n{icon:Action} The spell deals " + S.HeightenedVariable(spellLevel, 1) + "d4 sonic damage to a single enemy within 30ft, with a basic Fortitude save."
                    + "\r\n\r\n{icon:TwoActions} The spell deals " + S.HeightenedVariable(spellLevel * 2, 2) + "d4 sonic damage to all creatures in a 10-foot burst within 30ft, with a basic Fortitude save."
                    + "\r\n\r\n{icon:ThreeActions} The spell deals " + S.HeightenedVariable(spellLevel * 2, 2) + "d4 sonic damage to all creatures in a 30-foot emanation, with a basic Fortitude save."
                    + HS.HeightenTextLevels(spellLevel > 1, spellLevel, inCombat, "\n\n{b}Heightened (+1){/b} The damage increases by 1d4 for the 1-action version, or 2d4 for the other versions.")
                    ,
                    Target.DependsOnActionsSpent(
                        Target.Ranged(6),
                        Target.Burst(6, 2),
                        Target.SelfExcludingEmanation(6)),
                        spellLevel,
                        SpellSavingThrow.Basic(Defense.Fortitude)
                        ).WithActionCost(-1)
                        .WithCreateVariantDescription((int actionCost, SpellVariant _) => actionCost switch
                    {
                        1 => "{icon:Action} The spell deals " + S.HeightenedVariable(spellLevel, 1) + "d4 sonic damage to a single enemy within 30ft, with a basic Fortitude save."
                    + HS.HeightenTextLevels(spellLevel > 1, spellLevel, inCombat, "\n\n{b}Heightened (+1){/b} The damage increases by 1d4 for the 1-action version, or 2d4 for the other versions."),
                        2 => "{icon:TwoActions} The spell deals " + S.HeightenedVariable(spellLevel * 2, 2) + "d4 sonic damage to all creatures in a 10-foot burst within 30ft, with a basic Fortitude save."
                    + HS.HeightenTextLevels(spellLevel > 1, spellLevel, inCombat, "\n\n{b}Heightened (+1){/b} The damage increases by 1d4 for the 1-action version, or 2d4 for the other versions."),
                        3 => "{icon:ThreeActions} The spell deals " + S.HeightenedVariable(spellLevel * 2, 2) + "d4 sonic damage to all creatures in a 30-foot emanation, with a basic Fortitude save."
                    + HS.HeightenTextLevels(spellLevel > 1, spellLevel, inCombat, "\n\n{b}Heightened (+1){/b} The damage increases by 1d4 for the 1-action version, or 2d4 for the other versions."),
                        _ => null,
                    })
                        .WithGoodnessAgainstEnemy((Target t, Creature a, Creature d) => (float)t.OwnerAction.ActionCost >= 2 ? t.OwnerAction.SpellLevel * 2 * 2.5f : t.OwnerAction.SpellLevel * 2.5f)
                        .WithSoundEffect(SfxName.HauntingHymn)
                        .WithEffectOnEachTarget((Delegates.EffectOnEachTarget)(async (spell, caster, target, result) =>
                        {
                            string _Damage = ((spellLevel) * 2) + "d4";

                            if (spell.SpentActions == 1)
                            {
                                _Damage = spellLevel + "d4";
                            }


                            await CommonSpellEffects.DealBasicDamage(spell, caster, target, result, _Damage, DamageKind.Sonic);

                        }));


            return ConcordantChoirSpell;


        }
    }
    public static void LoadMod()
    {
        try
        {
            Id = ModManager.RegisterNewSpell("ConcordantChoir", 1, (spellId, spellcaster, spellLevel, inCombat, SpellInformation) => MakeConcordantChoirSpell(spellcaster, spellLevel, inCombat));
        }
        catch (System.ArgumentException ex)
        {
            Dawnsbury.IO.GeneralLog.Log("Skipped registering spell \"ConcordantChoir\". This spell may already be provided by another mod.\n" + ex.ToString());
            Id = SpellId.None;
        }
    }
}


