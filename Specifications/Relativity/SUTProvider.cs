/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 * --------------------------------------------------------------------------------------------*/

namespace Dolittle.Runtime.Events.Relativity.Specs.EventStore
{
    public class SUTProvider : IProvideGeodesics
    {
        public IGeodesics Build() => new Geodesics();
    }
}