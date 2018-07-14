using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Babreones Character Combat is used to define multiple Combat Related Systems that requires their own
/// Scripts for their independent functionalities
/// </summary>
namespace Barebones.DamageSystem
{
    // Determines what kind of damage is being received
    public enum Type
    {
        Normal = 0,
        Poison = 1,
    }
    [Serializable]
    public class BareboneDamage {
       // Determines how the damage would be dealt.
       public enum Process
        {
            Single = 0,
            Overtime = 1,
        }
        [SerializeField] private string name;
        [SerializeField] private Type damageType;
        [SerializeField] private Process damageProcess;
        [SerializeField] private int tickCount;
        private float dotTimer;
        [SerializeField] private float damageOvertime;
        [SerializeField] private float minimumDamage;
        [SerializeField] private float maximumDamage;


        public void Initialize()
        {
            //Debug.Log(name);
            this.name = this.damageType.ToString();
        }
        public float DotTimer
        {
            get
            {
                return this.dotTimer;
            }
            set
            {
                this.dotTimer = value;
            }
        }

        public int TickCount
        {
            get
            {
                return this.tickCount;
            }
            set
            {
                this.tickCount = value;
            }
        }
        public string Name
        {
            get
            {
                //Debug.Log(this.name);
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public Type GetDamageType
        {
            get
            {
                return this.damageType;
            }
        }
        public Process GetDamageProcess
        {
            get
            {
                return this.damageProcess;
            }
        }
        public float Overtimedamage
        {
            get
            {
                return this.damageOvertime;
            }
            set
            {
                this.damageOvertime = value;
            }
        }
        public float MinimumDamage
        {
            get
            {
                return this.minimumDamage;
            }
            set
            {
                this.minimumDamage = value;
            }
        }
        public float MaximumDamage
        {
            get
            {
                return this.maximumDamage;
            }
            set
            {
                this.maximumDamage = value;
            }
        }

        public BareboneDamage Copy(BareboneDamage bareboneDamage)
        {
            BareboneDamage tmp = new BareboneDamage();
            tmp.maximumDamage = bareboneDamage.maximumDamage;
            tmp.minimumDamage = bareboneDamage.minimumDamage;
            tmp.damageOvertime = bareboneDamage.damageOvertime;
            tmp.damageType = bareboneDamage.damageType;
            tmp.damageProcess = bareboneDamage.damageProcess;
            tmp.name = bareboneDamage.Name;
            tmp.tickCount = bareboneDamage.tickCount;

            return tmp;
        }
    }
}
