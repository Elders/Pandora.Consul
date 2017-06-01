namespace Elders.Pandora
{
    public static class PandoraKeyExtentions
    {
        public static string ToConsulKey(this string rawKey)
        {
            var key = Key.Parse(rawKey);
            return $"pandora/{key.ApplicationName}/{key.Cluster}/{key.Machine}/{key.SettingKey}";
        }

        public static Key FromConsulKey(this string consulKey)
        {
            string[] parts = consulKey.Split('/');
            return new Key(parts[1], parts[2], parts[3], parts[4]);
        }
    }
}
