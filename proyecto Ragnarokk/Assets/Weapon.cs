using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//luego las armas deben tener mas caracteristicas.
[CreateAssetMenu(menuName = "Combat/Weapon")]
public class Weapon : ScriptableObject
{
    public string Name = "unnamed weapon";
    public int BaseDamage = 50;
    [Range(0, 100)]
    public int BaseAccuracy;
    public int BaseCooldown;
    //[HideInInspector] public int CurrentCooldown;
    public Sprite sprite;

    //deberia tener la info de:

    //Los estados que aplica.
    public List<CombatState> ListaDeEstadosQueAplica;

    //Tipo
    public CombatType TipoDeDañoQueAplica;

}
