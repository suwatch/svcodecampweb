//  This is the Manager class used for data operations.  It is meant to have another Partial
//  class associated with it.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using CodeCampSV;


namespace CodeCampSV
{
    //  Here are the 2 methods that needs to be auto genearted. 
    //  First is a one to one maping to the database columns. 
    //  Since we auto generate the results class too, we can guarantee the columns are all there
    [DataObject(true)]
    public partial class ZIPCODEWORLDGOLDManager : ManagerBase<ZIPCODEWORLDGOLDManager, ZIPCODEWORLDGOLDResult, ZIPCODEWORLDGOLD, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(ZIPCODEWORLDGOLD record, ZIPCODEWORLDGOLDResult result)
        {
            record.ZIP_CODE = result.ZIP_CODE;
            record.CITY = result.CITY;
            record.STATE = result.STATE;
            record.AREA_CODE = result.AREA_CODE;
            record.CITY_ALIAS_NAME = result.CITY_ALIAS_NAME;
            record.CITY_ALIAS_ABBR = result.CITY_ALIAS_ABBR;
            record.CITY_TYPE = result.CITY_TYPE;
            record.COUNTY_NAME = result.COUNTY_NAME;
            record.STATE_FIPS = result.STATE_FIPS;
            record.COUNTY_FIPS = result.COUNTY_FIPS;
            record.TIME_ZONE = result.TIME_ZONE;
            record.DAY_LIGHT_SAVING = result.DAY_LIGHT_SAVING;
            record.LATITUDE = result.LATITUDE;
            record.LONGITUDE = result.LONGITUDE;
            record.ELEVATION = result.ELEVATION;
            record.MSA2000 = result.MSA2000;
            record.PMSA = result.PMSA;
            record.CBSA = result.CBSA;
            record.CBSA_DIV = result.CBSA_DIV;
            record.CBSA_TITLE = result.CBSA_TITLE;
            record.PERSONS_PER_HOUSEHOLD = result.PERSONS_PER_HOUSEHOLD;
            record.ZIPCODE_POPULATION = result.ZIPCODE_POPULATION;
            record.COUNTIES_AREA = result.COUNTIES_AREA;
            record.HOUSEHOLDS_PER_ZIPCODE = result.HOUSEHOLDS_PER_ZIPCODE;
            record.WHITE_POPULATION = result.WHITE_POPULATION;
            record.BLACK_POPULATION = result.BLACK_POPULATION;
            record.HISPANIC_POPULATION = result.HISPANIC_POPULATION;
            record.INCOME_PER_HOUSEHOLD = result.INCOME_PER_HOUSEHOLD;
            record.AVERAGE_HOUSE_VALUE = result.AVERAGE_HOUSE_VALUE;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override ZIPCODEWORLDGOLD GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.ZIPCODEWORLDGOLD where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<ZIPCODEWORLDGOLDResult> GetBaseResultIQueryable(IQueryable<ZIPCODEWORLDGOLD> baseQuery)
        {
      IQueryable<ZIPCODEWORLDGOLDResult> results = (from myData in baseQuery orderby myData.Id select new ZIPCODEWORLDGOLDResult { Id= myData.Id,
            ZIP_CODE = myData.ZIP_CODE,
            CITY = myData.CITY,
            STATE = myData.STATE,
            AREA_CODE = myData.AREA_CODE,
            CITY_ALIAS_NAME = myData.CITY_ALIAS_NAME,
            CITY_ALIAS_ABBR = myData.CITY_ALIAS_ABBR,
            CITY_TYPE = myData.CITY_TYPE,
            COUNTY_NAME = myData.COUNTY_NAME,
            STATE_FIPS = myData.STATE_FIPS,
            COUNTY_FIPS = myData.COUNTY_FIPS,
            TIME_ZONE = myData.TIME_ZONE,
            DAY_LIGHT_SAVING = myData.DAY_LIGHT_SAVING,
            LATITUDE = myData.LATITUDE,
            LONGITUDE = myData.LONGITUDE,
            ELEVATION = myData.ELEVATION,
            MSA2000 = myData.MSA2000,
            PMSA = myData.PMSA,
            CBSA = myData.CBSA,
            CBSA_DIV = myData.CBSA_DIV,
            CBSA_TITLE = myData.CBSA_TITLE,
            PERSONS_PER_HOUSEHOLD = myData.PERSONS_PER_HOUSEHOLD,
            ZIPCODE_POPULATION = myData.ZIPCODE_POPULATION,
            COUNTIES_AREA = myData.COUNTIES_AREA,
            HOUSEHOLDS_PER_ZIPCODE = myData.HOUSEHOLDS_PER_ZIPCODE,
            WHITE_POPULATION = myData.WHITE_POPULATION,
            BLACK_POPULATION = myData.BLACK_POPULATION,
            HISPANIC_POPULATION = myData.HISPANIC_POPULATION,
            INCOME_PER_HOUSEHOLD = myData.INCOME_PER_HOUSEHOLD,
            AVERAGE_HOUSE_VALUE = myData.AVERAGE_HOUSE_VALUE
      });
		    return results;
        }
        
        public List<ZIPCODEWORLDGOLDResult> GetJustBaseTableColumns(ZIPCODEWORLDGOLDQuery query)
        {
            foreach (var info in typeof (ZIPCODEWORLDGOLDQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: ZIPCODEWORLDGOLD QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<ZIPCODEWORLDGOLD> baseQuery = from myData in meta.ZIPCODEWORLDGOLD select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<ZIPCODEWORLDGOLDResult> results = (from myData in baseQuery orderby myData.Id select new ZIPCODEWORLDGOLDResult { Id= myData.Id,
                        ZIP_CODE = myData.ZIP_CODE,
                        CITY = myData.CITY,
                        STATE = myData.STATE,
                        AREA_CODE = myData.AREA_CODE,
                        CITY_ALIAS_NAME = myData.CITY_ALIAS_NAME,
                        CITY_ALIAS_ABBR = myData.CITY_ALIAS_ABBR,
                        CITY_TYPE = myData.CITY_TYPE,
                        COUNTY_NAME = myData.COUNTY_NAME,
                        STATE_FIPS = myData.STATE_FIPS,
                        COUNTY_FIPS = myData.COUNTY_FIPS,
                        TIME_ZONE = myData.TIME_ZONE,
                        DAY_LIGHT_SAVING = myData.DAY_LIGHT_SAVING,
                        LATITUDE = myData.LATITUDE,
                        LONGITUDE = myData.LONGITUDE,
                        ELEVATION = myData.ELEVATION,
                        MSA2000 = myData.MSA2000,
                        PMSA = myData.PMSA,
                        CBSA = myData.CBSA,
                        CBSA_DIV = myData.CBSA_DIV,
                        CBSA_TITLE = myData.CBSA_TITLE,
                        PERSONS_PER_HOUSEHOLD = myData.PERSONS_PER_HOUSEHOLD,
                        ZIPCODE_POPULATION = myData.ZIPCODE_POPULATION,
                        COUNTIES_AREA = myData.COUNTIES_AREA,
                        HOUSEHOLDS_PER_ZIPCODE = myData.HOUSEHOLDS_PER_ZIPCODE,
                        WHITE_POPULATION = myData.WHITE_POPULATION,
                        BLACK_POPULATION = myData.BLACK_POPULATION,
                        HISPANIC_POPULATION = myData.HISPANIC_POPULATION,
                        INCOME_PER_HOUSEHOLD = myData.INCOME_PER_HOUSEHOLD,
                        AVERAGE_HOUSE_VALUE = myData.AVERAGE_HOUSE_VALUE
            });
            
            List<ZIPCODEWORLDGOLDResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<ZIPCODEWORLDGOLD> BaseQueryAutoGen(IQueryable<ZIPCODEWORLDGOLD> baseQuery, ZIPCODEWORLDGOLDQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.ZIP_CODE != null) baseQuery = baseQuery.Where(a => a.ZIP_CODE.ToLower().Equals(query.ZIP_CODE.ToLower()));
            if (query.CITY != null) baseQuery = baseQuery.Where(a => a.CITY.ToLower().Equals(query.CITY.ToLower()));
            if (query.STATE != null) baseQuery = baseQuery.Where(a => a.STATE.ToLower().Equals(query.STATE.ToLower()));
            if (query.AREA_CODE != null) baseQuery = baseQuery.Where(a => a.AREA_CODE.ToLower().Equals(query.AREA_CODE.ToLower()));
            if (query.CITY_ALIAS_NAME != null) baseQuery = baseQuery.Where(a => a.CITY_ALIAS_NAME.ToLower().Equals(query.CITY_ALIAS_NAME.ToLower()));
            if (query.CITY_ALIAS_ABBR != null) baseQuery = baseQuery.Where(a => a.CITY_ALIAS_ABBR.ToLower().Equals(query.CITY_ALIAS_ABBR.ToLower()));
            if (query.CITY_TYPE != null) baseQuery = baseQuery.Where(a => a.CITY_TYPE.ToLower().Equals(query.CITY_TYPE.ToLower()));
            if (query.COUNTY_NAME != null) baseQuery = baseQuery.Where(a => a.COUNTY_NAME.ToLower().Equals(query.COUNTY_NAME.ToLower()));
            if (query.STATE_FIPS != null) baseQuery = baseQuery.Where(a => a.STATE_FIPS.ToLower().Equals(query.STATE_FIPS.ToLower()));
            if (query.COUNTY_FIPS != null) baseQuery = baseQuery.Where(a => a.COUNTY_FIPS.ToLower().Equals(query.COUNTY_FIPS.ToLower()));
            if (query.TIME_ZONE != null) baseQuery = baseQuery.Where(a => a.TIME_ZONE.ToLower().Equals(query.TIME_ZONE.ToLower()));
            if (query.DAY_LIGHT_SAVING != null) baseQuery = baseQuery.Where(a => a.DAY_LIGHT_SAVING.ToLower().Equals(query.DAY_LIGHT_SAVING.ToLower()));
            if (query.LATITUDE != null) baseQuery = baseQuery.Where(a => a.LATITUDE.ToLower().Equals(query.LATITUDE.ToLower()));
            if (query.LONGITUDE != null) baseQuery = baseQuery.Where(a => a.LONGITUDE.ToLower().Equals(query.LONGITUDE.ToLower()));
            if (query.ELEVATION != null) baseQuery = baseQuery.Where(a => a.ELEVATION.ToLower().Equals(query.ELEVATION.ToLower()));
            if (query.MSA2000 != null) baseQuery = baseQuery.Where(a => a.MSA2000.ToLower().Equals(query.MSA2000.ToLower()));
            if (query.PMSA != null) baseQuery = baseQuery.Where(a => a.PMSA.ToLower().Equals(query.PMSA.ToLower()));
            if (query.CBSA != null) baseQuery = baseQuery.Where(a => a.CBSA.ToLower().Equals(query.CBSA.ToLower()));
            if (query.CBSA_DIV != null) baseQuery = baseQuery.Where(a => a.CBSA_DIV.ToLower().Equals(query.CBSA_DIV.ToLower()));
            if (query.CBSA_TITLE != null) baseQuery = baseQuery.Where(a => a.CBSA_TITLE.ToLower().Equals(query.CBSA_TITLE.ToLower()));
            if (query.PERSONS_PER_HOUSEHOLD != null) baseQuery = baseQuery.Where(a => a.PERSONS_PER_HOUSEHOLD.ToLower().Equals(query.PERSONS_PER_HOUSEHOLD.ToLower()));
            if (query.ZIPCODE_POPULATION != null) baseQuery = baseQuery.Where(a => a.ZIPCODE_POPULATION.ToLower().Equals(query.ZIPCODE_POPULATION.ToLower()));
            if (query.COUNTIES_AREA != null) baseQuery = baseQuery.Where(a => a.COUNTIES_AREA.ToLower().Equals(query.COUNTIES_AREA.ToLower()));
            if (query.HOUSEHOLDS_PER_ZIPCODE != null) baseQuery = baseQuery.Where(a => a.HOUSEHOLDS_PER_ZIPCODE.ToLower().Equals(query.HOUSEHOLDS_PER_ZIPCODE.ToLower()));
            if (query.WHITE_POPULATION != null) baseQuery = baseQuery.Where(a => a.WHITE_POPULATION.ToLower().Equals(query.WHITE_POPULATION.ToLower()));
            if (query.BLACK_POPULATION != null) baseQuery = baseQuery.Where(a => a.BLACK_POPULATION.ToLower().Equals(query.BLACK_POPULATION.ToLower()));
            if (query.HISPANIC_POPULATION != null) baseQuery = baseQuery.Where(a => a.HISPANIC_POPULATION.ToLower().Equals(query.HISPANIC_POPULATION.ToLower()));
            if (query.INCOME_PER_HOUSEHOLD != null) baseQuery = baseQuery.Where(a => a.INCOME_PER_HOUSEHOLD.ToLower().Equals(query.INCOME_PER_HOUSEHOLD.ToLower()));
            if (query.AVERAGE_HOUSE_VALUE != null) baseQuery = baseQuery.Where(a => a.AVERAGE_HOUSE_VALUE.ToLower().Equals(query.AVERAGE_HOUSE_VALUE.ToLower()));

            return baseQuery;
        }
        
    }
}
