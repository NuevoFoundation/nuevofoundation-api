using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using API.Models;
using AutoMapper;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class MetaDataController : ControllerBase
    {

        private readonly IStorage<MetaData> _storage;
        private readonly IMapper _mapper;

        public MetaDataController(IStorage<MetaData> storage, IMapper mapper)
        {
            this._storage = storage;
            _mapper = mapper;
        }

        // GET api/metadata
        [HttpGet]
        public async Task<ActionResult<MetaDataDto>> GetMetaData()
        {
            var data = await _storage.GetItemsAsync();
            var response = BuildMetaDataResponse(data);
            return Ok(response);
        }

        // POST api/metadata
        #if DEBUG
        [HttpPost]
        public async Task<ActionResult<MetaDataDto>> PostMetaData([FromBody]MetaData metaData)
        {
            metaData.Id = Guid.NewGuid();
            var data = await _storage.CreateItemAsync(metaData);
            return Ok();
        }
        #endif

        private MetaDataDto BuildMetaDataResponse(IEnumerable<MetaData> data)
        {
            var metaDataDto = new MetaDataDto()
            {
                Team = new List<TeamDto>(),
                ImpactStats = new List<ImpactStatsDto>()
            };

            foreach (var stat in data)
            {
                switch (stat.Section)
                {
                    case "team":
                        metaDataDto.Team.Add(_mapper.Map<TeamDto>(stat));
                        break;
                    case "impact":
                        metaDataDto.ImpactStats.Add(_mapper.Map<ImpactStatsDto>(stat));
                        break;
                    default:
                        break;
                }
            }

            return metaDataDto;
        }
    }
}
