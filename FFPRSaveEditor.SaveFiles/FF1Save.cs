using System.Text.Json;
using System.Text.Json.Serialization;

namespace FFPRSaveEditor.SaveFiles
{
    public class FF1Save
    {
        public static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        [JsonPropertyName("id")]
        public SaveSlot Id { get; set; }

        [JsonPropertyName("userData")]
        public string UserDataString { get; set; }

        [JsonIgnore]
        public FF1UserData UserDataValue { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JsonElement>? AdditionalProperties { get; set; }

        public static FF1Save Deserialize(string json)
        {
            var save = JsonSerializer.Deserialize<FF1Save>(json);
            if (save == null)
            {
                throw new ArgumentException("invalid json", "json");
            }

            save.UserDataValue = FF1UserData.Deserialize(save.UserDataString);
            return save;
        }

        public string Serialize()
        {
            var output = new FF1Save();
            output.AdditionalProperties = AdditionalProperties;
            output.Id = Id;
            output.UserDataString = UserDataValue.Serialize();

            return JsonSerializer.Serialize(output, options: serializerOptions); 
        }
    }

    public class FF1UserData
    {
        [JsonPropertyName("ownedCharacterList")]
        public string OwnedCharacterListString { get; set; } = "";

        [JsonIgnore]
        public FF1CharacterList? OwnedCharacterListValue { get; set; }

        [JsonPropertyName("owendGil")]
        public int Gil { get; set; }

        [JsonPropertyName("normalOwnedItemList")]
        public string OwnedNormalItemsString { get; set; }

        /// <summary>
        /// Includes equipped items and items in the inventory
        /// There is a second list that shows the sort order
        /// If items are here but missing there they go to the bottom
        /// so I think I'll just leave it off as it isn't required for things
        /// to work
        /// </summary>
        [JsonIgnore]
        public FF1ItemList OwnedNormalItems { get; set; }

        [JsonPropertyName("importantOwendItemList")]
        public string KeyItemsString { get; set; }

        [JsonIgnore]
        public FF1ItemList KeyItems { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JsonElement>? AdditionalProperties { get; set; }

        internal static FF1UserData Deserialize(string json)
        {
            var userData = JsonSerializer.Deserialize<FF1UserData>(json);
            if (userData == null)
            {
                throw new ArgumentException("invalid json", "json");
            }

            userData.OwnedCharacterListValue = FF1CharacterList.Deserialize(userData.OwnedCharacterListString);
            userData.KeyItems = FF1ItemList.Deserialize(userData.KeyItemsString);
            userData.OwnedNormalItems = FF1ItemList.Deserialize(userData.OwnedNormalItemsString);

            return userData;
        }

        internal string Serialize()
        {
            var output = new FF1UserData();
            output.AdditionalProperties = AdditionalProperties;
            output.Gil = Gil;
            output.OwnedCharacterListString = OwnedCharacterListValue.Serialize();
            output.KeyItemsString = KeyItems.Serialize();
            output.OwnedNormalItemsString = OwnedNormalItems.Serialize();

            return JsonSerializer.Serialize(output, options: FF1Save.serializerOptions);
        }
    }

    public class FF1CharacterList
    {
        [JsonPropertyName("target")]
        public List<string> CharacterListString { get; set; } = new List<string>();

        [JsonIgnore]
        public List<FF1Character> CharacterListValue { get; set; } = new List<FF1Character>();

        internal static FF1CharacterList Deserialize(string json)
        {
            var list = JsonSerializer.Deserialize<FF1CharacterList>(json);
            if (list == null)
            {
                throw new ArgumentException("invalid json", "json");
            }

            foreach (var characterString in list.CharacterListString)
            {
                list.CharacterListValue.Add(FF1Character.Deserialize(characterString));
            }

            return list;
        }

        public string Serialize()
        {
            var output = new FF1CharacterList();
            foreach(var character in CharacterListValue)
            {
                output.CharacterListString.Add(character.Serialize());
            }

            return JsonSerializer.Serialize(output, options: FF1Save.serializerOptions);
        }
    }

    public class FF1Character
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("jobId")]
        public FF1CharacterJob Job { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("currentExp")]
        public int Experience { get; set; }

        [JsonPropertyName("parameter")]
        public string ParameterString { get; set; }

        [JsonIgnore]
        public FF1CharacterParameters ParameterValue { get; set; }

        [JsonPropertyName("equipmentList")]
        public string EquipmentListString { get; set; }

        [JsonIgnore]
        public FF1EquipmentList EquipmentListValue { get; set; }

        // TODO handle the 3 properties that combine to determine the spell list

        // TODO handle the list of abilities for the character
        // probably needs to be updated if a job is changed

        [JsonExtensionData]
        public IDictionary<string, JsonElement>? AdditionalProperties { get; set; }

        internal static FF1Character Deserialize(string json)
        {
            var character = JsonSerializer.Deserialize<FF1Character>(json);
            if (character == null)
            {
                throw new ArgumentException("invalid json", "json");
            }

            character.ParameterValue = FF1CharacterParameters.Deserialize(character.ParameterString);
            character.EquipmentListValue = FF1EquipmentList.Deserialize(character.EquipmentListString);
            return character;
        }

        internal string Serialize()
        {
            var output = new FF1Character();
            output.AdditionalProperties = AdditionalProperties;
            output.Id = Id;
            output.Job = Job;
            output.Name = Name;
            output.Experience = Experience;
            output.ParameterString = ParameterValue.Serialize();
            output.EquipmentListString = EquipmentListValue.Serialize();

            return JsonSerializer.Serialize(output, options: FF1Save.serializerOptions);
        }
    }

    /// <summary>
    /// Parameters about a specific character
    /// This includes stat gains from levels, Max mp charges and current mp charges
    /// This has a number of stats that don't show up on the status page.
    /// Magic, Spirit, Ability Evasion Rate
    /// I didn't make first class fields for them as they don't show on the stats page
    /// and I'm unsure if they actually do anything
    /// </summary>
    public class FF1CharacterParameters
    {
        /// <summary>
        /// Current Hp of Character
        /// </summary>
        [JsonPropertyName("currentHP")]
        public int CurrentHp { get; set; }

        [JsonPropertyName("addtionalLevel")]
        public int Level { get; set; }

        /// <summary>
        /// Max Hp gained from levels on top of base Hp
        /// </summary>
        [JsonPropertyName("addtionalMaxHp")]
        public int ExtraMaxHp { get; set; }

        /// <summary>
        /// Strength gained from levels on top of base strength
        /// </summary>
        [JsonPropertyName("addtionalPower")]
        public int ExtraStrength { get; set; }

        /// <summary>
        /// Stamina gained from levels on top of base Stamina
        /// </summary>
        [JsonPropertyName("addtionalVitality")]
        public int ExtraStamina { get; set; }

        /// <summary>
        /// Agility gained from levels on top of base Agility
        /// </summary>
        [JsonPropertyName("addtionalAgility")]
        public int ExtraAgility { get; set; }

        /// <summary>
        /// Intelligence gained from levels on top of base Intelligence
        /// </summary>
        [JsonPropertyName("addtionalIntelligence")]
        public int ExtraInt { get; set; }

        /// <summary>
        /// Luck gained from levels on top of base Luck
        /// </summary>
        [JsonPropertyName("addtionalLuck")]
        public int ExtraLuck { get; set; }

        /// <summary>
        /// Accuracy gained from levels, unclear if this includes the weapon accuracy I think not
        /// </summary>
        [JsonPropertyName("addtionalAccuracyRate")]
        public int ExtraAccuracy { get; set; }

        [JsonPropertyName("currentMpCountList")]
        public string CurrentMpChargesListString { get; set; }

        [JsonIgnore]
        public FF1MpChargesList CurrentMpCharges { get; set; }

        [JsonPropertyName("addtionalMaxMpCountList")]
        public string ExtraMaxMpChargesListString { get; set; }

        /// <summary>
        /// Max Mp charges for the character on top of the base values
        /// </summary>
        [JsonIgnore]
        public FF1MpChargesList ExtraMaxMpCharges { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JsonElement>? AdditionalProperties { get; set; }

        internal static FF1CharacterParameters Deserialize(string json)
        {
            var parameters = JsonSerializer.Deserialize<FF1CharacterParameters>(json);
            if (parameters == null)
            {
                throw new ArgumentException("invalid json", "json");
            }

            parameters.CurrentMpCharges = JsonSerializer.Deserialize<FF1MpChargesList>(parameters.CurrentMpChargesListString);
            parameters.ExtraMaxMpCharges = JsonSerializer.Deserialize<FF1MpChargesList>(parameters.ExtraMaxMpChargesListString);

            return parameters;
        }

        internal string Serialize()
        {
            var output = new FF1CharacterParameters();
            output.AdditionalProperties = AdditionalProperties;
            output.CurrentHp = CurrentHp;
            output.Level = Level;
            output.ExtraAccuracy = ExtraAccuracy;
            output.ExtraAgility = ExtraAgility;
            output.ExtraInt = ExtraInt;
            output.ExtraLuck = ExtraLuck;
            output.ExtraMaxHp = ExtraMaxHp;
            output.ExtraStamina = ExtraStamina;
            output.ExtraStrength = ExtraStrength;
            output.CurrentMpChargesListString = JsonSerializer.Serialize(CurrentMpCharges);
            output.ExtraMaxMpChargesListString = JsonSerializer.Serialize(ExtraMaxMpCharges);

            return JsonSerializer.Serialize(output, options: FF1Save.serializerOptions);
        }
    }

    public class FF1MpChargesList
    {
        /// <summary>
        /// Spell level for the mp charge count in the corresponding index in values
        /// </summary>
        [JsonPropertyName("keys")]
        public List<int> Keys { get; set; }

        /// <summary>
        /// Current Count of charges for the Spell level from keys list
        /// </summary>
        [JsonPropertyName("values")]
        public List<int> Values { get; set; }
    }

    public class FF1EquipmentList
    {
        [JsonPropertyName("keys")]
        public List<FF1EquipmentSlot> Slots { get; set; }

        [JsonPropertyName("values")]
        public List<string> EquippedItemsStrings { get; set; } = new List<string>();

        /// <summary>
        /// Counts on the items include equipped items and items in the inventory
        /// </summary>
        [JsonIgnore]
        public List<FF1ItemGroup> EquippedItems { get; set; } = new List<FF1ItemGroup>();

        internal static FF1EquipmentList Deserialize(string json)
        {
            var list = JsonSerializer.Deserialize<FF1EquipmentList>(json);
            if (list == null)
            {
                throw new ArgumentException("invalid JSON", "json");
            }

            foreach(var itemString in list.EquippedItemsStrings)
            {
                list.EquippedItems.Add(JsonSerializer.Deserialize<FF1ItemGroup>(itemString));
            }

            return list;
        }

        internal string Serialize()
        {
            var output = new FF1EquipmentList();
            output.Slots = Slots;
            foreach(var item in EquippedItems)
            {
                output.EquippedItemsStrings.Add(JsonSerializer.Serialize(item));
            }

            return JsonSerializer.Serialize(output, options: FF1Save.serializerOptions);
        }
    }

    public class FF1ItemList
    {
        [JsonPropertyName("target")]
        public List<string> ItemsString { get; set; } = new List<string>();

        [JsonIgnore]
        public List<FF1ItemGroup> Items { get; set; } = new List<FF1ItemGroup>();

        public static FF1ItemList Deserialize(string json)
        {
            var list = JsonSerializer.Deserialize<FF1ItemList>(json);
            if (list == null)
            {
                throw new ArgumentException("invalid json", "json");
            }

            foreach (var itemString in list.ItemsString)
            {
                list.Items.Add(JsonSerializer.Deserialize<FF1ItemGroup>(itemString));
            }

            return list;
        }

        internal string Serialize()
        {
            var output = new FF1ItemList();
            foreach(var item in Items)
            {
                output.ItemsString.Add(JsonSerializer.Serialize(item));
            }

            return JsonSerializer.Serialize(output, options: FF1Save.serializerOptions);
        }
    }

    public class FF1ItemGroup
    {
        [JsonPropertyName("contentId")]
        public FF1ItemType ItemType { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}