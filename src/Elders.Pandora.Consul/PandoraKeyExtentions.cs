﻿namespace Elders.Pandora
{
    public static class PandoraKeyExtentions
    {
        public static string ToConsulKey(this string rawKey)
        {
            var key = Key.Parse(rawKey);
            return $"{ConsulForPandora.RootFolder}/{key.ApplicationName}/{key.Cluster}/{key.Machine}/{key.SettingKey}";
        }

        public static string ToApplicationKeyPrefix(this IPandoraContext context)
        {
            return $"{ConsulForPandora.RootFolder}/{context.ApplicationName}".ToLower();
        }

        public static Key FromConsulKey(this string consulKey)
        {
            string[] parts = consulKey.Split('/');
            if (parts.Length != 5)
            {
                return null;
            }
            return new Key(parts[1], parts[2], parts[3], parts[4]);
        }
    }
}
