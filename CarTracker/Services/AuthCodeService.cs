using Microsoft.Extensions.Caching.Memory;

namespace CarTracker.Services;

public class AuthCodeService : IAuthService
{
   private readonly IMemoryCache _cache;

   public AuthCodeService(IMemoryCache cache)
   {
      _cache = cache;
   }

   public string GenerateCode(long telegramId)
   {
      var code = Random.Shared.Next(100000, 999999).ToString();
      
      _cache.Set(code, telegramId, TimeSpan.FromMinutes(5));
      
      return code;
   }

   public long? ValidateCode(string code)
   {
      if (_cache.TryGetValue(code, out long telegramId))
      {
         _cache.Remove(code);
         return telegramId;
      }
      
      return null;
   }
}